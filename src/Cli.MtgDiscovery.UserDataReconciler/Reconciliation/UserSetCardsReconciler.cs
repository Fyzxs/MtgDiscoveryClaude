using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cli.MtgDiscovery.UserDataReconciler.Dashboard;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Entities;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitors;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Scribes;
using Lib.Cosmos.Apis.Operators;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;

namespace Cli.MtgDiscovery.UserDataReconciler.Reconciliation;

internal sealed class UserSetCardsReconciler
{
    private readonly ILogger _logger;
    private readonly bool _dryRun;
    private readonly IReconcilerDashboard _dashboard;
    private readonly UserCardsInquisitor _userCardsInquisitor;
    private readonly UserSetCardsInquisitor _userSetCardsInquisitor;
    private readonly UserSetCardsScribe _userSetCardsScribe;
    private readonly ScryfallSetCardsInquisitor _setCardsInquisitor;

    public UserSetCardsReconciler(ILogger logger, bool dryRun, IReconcilerDashboard dashboard)
    {
        _logger = logger;
        _dryRun = dryRun;
        _dashboard = dashboard;
        _userCardsInquisitor = new UserCardsInquisitor(logger);
        _userSetCardsInquisitor = new UserSetCardsInquisitor(logger);
        _userSetCardsScribe = new UserSetCardsScribe(logger);
        _setCardsInquisitor = new ScryfallSetCardsInquisitor(logger);
    }

    public async Task<ReconciliationSummary> ReconcileUserAsync(string userId)
    {
        ReconciliationSummary summary = new();

        List<UserCardExtEntity> userCards = await GetUserCardsAsync(userId).ConfigureAwait(false);
        if (userCards.Count == 0)
        {
            _dashboard.UpdateUserProgress("No cards", 0, 0);
            return summary;
        }

        _dashboard.AddLog($"Loaded {userCards.Count} cards");

        List<UserSetCardExtEntity> userSetCards = await GetUserSetCardsAsync(userId).ConfigureAwait(false);

        ILookup<string, UserCardExtEntity> cardsBySetId = userCards.ToLookup(c => c.SetId);
        int totalSets = cardsBySetId.Count;

        _dashboard.AddLog($"Processing {totalSets} sets...");

        int setIndex = 0;
        foreach (IGrouping<string, UserCardExtEntity> setGroup in cardsBySetId)
        {
            string setId = setGroup.Key;
            List<UserCardExtEntity> cardsInSet = setGroup.ToList();

            setIndex++;
            _dashboard.UpdateUserProgress(setId, setIndex, totalSets);
            _dashboard.IncrementSetsProcessed();

            summary.SetsProcessed++;

            Dictionary<string, string> cardIdToSetGroupId = await GetSetGroupIdsForSetAsync(setId).ConfigureAwait(false);

            // Log what group IDs were found for this set
            HashSet<string> groupIds = [.. cardIdToSetGroupId.Values.Distinct()];
            if (groupIds.Count > 0)
            {
                _dashboard.AddLog($"{setId}: groups found = [{string.Join(", ", groupIds)}]");
            }

            UserSetCardExtEntity existingSetCard = userSetCards.FirstOrDefault(usc => usc.SetId == setId);

            ReconcileSetResult result = await ReconcileSetAsync(
                userId,
                setId,
                cardsInSet,
                existingSetCard,
                cardIdToSetGroupId).ConfigureAwait(false);

            if (result.HasDiscrepancy)
            {
                summary.SetsWithDiscrepancies++;
                summary.CardsAddedToGroups += result.CardsAdded;
                summary.CardsRemovedFromGroups += result.CardsRemoved;

                _dashboard.IncrementSetsWithDiscrepancy();
                _dashboard.AddCardsAdded(result.CardsAdded);
                _dashboard.AddCardsRemoved(result.CardsRemoved);

                string changeDescription = BuildChangeDescription(result);

                if (result.Updated)
                {
                    summary.SetsUpdated++;
                    _dashboard.IncrementSetsUpdated();
                    _dashboard.AddLog($"Updated {setId}: {changeDescription}");
                }
                else if (_dryRun)
                {
                    _dashboard.AddLog($"Would fix {setId}: {changeDescription}");
                }
            }

            if (result.HasError)
            {
                summary.Errors++;
                _dashboard.IncrementError();
                _dashboard.AddLog($"ERROR updating {setId}");
            }
        }

        return summary;
    }

    private async Task<List<UserCardExtEntity>> GetUserCardsAsync(string userId)
    {
        QueryDefinition query = new("SELECT * FROM c WHERE c.partition = @userId");
        query = query.WithParameter("@userId", userId);

        PartitionKey partitionKey = new(userId);
        OpResponse<IEnumerable<UserCardExtEntity>> response = await _userCardsInquisitor
            .QueryAsync<UserCardExtEntity>(query, partitionKey)
            .ConfigureAwait(false);

        if (response.IsNotSuccessful())
        {
            _logger.LogError("Failed to query UserCards for user {UserId}: {StatusCode}", userId, response.StatusCode);
            return [];
        }

        return response.Value.ToList();
    }

    private async Task<List<UserSetCardExtEntity>> GetUserSetCardsAsync(string userId)
    {
        QueryDefinition query = new("SELECT * FROM c WHERE c.partition = @userId");
        query = query.WithParameter("@userId", userId);

        PartitionKey partitionKey = new(userId);
        OpResponse<IEnumerable<UserSetCardExtEntity>> response = await _userSetCardsInquisitor
            .QueryAsync<UserSetCardExtEntity>(query, partitionKey)
            .ConfigureAwait(false);

        if (response.IsNotSuccessful())
        {
            _logger.LogError("Failed to query UserSetCards for user {UserId}: {StatusCode}", userId, response.StatusCode);
            return [];
        }

        return response.Value.ToList();
    }

    private async Task<Dictionary<string, string>> GetSetGroupIdsForSetAsync(string setId)
    {
        Dictionary<string, string> map = [];

        // Debug: For hml specifically, check what fields exist in data
        if (setId == "hml")
        {
            await DebugQueryHomelandsAsync(setId).ConfigureAwait(false);
        }

        QueryDefinition query = new("SELECT c.id AS id, c.data.set_group_id AS set_group_id FROM c WHERE c.partition = @setId");
        query = query.WithParameter("@setId", setId);

        PartitionKey partitionKey = new(setId);
        OpResponse<IEnumerable<SetCardGroupIdResult>> response = await _setCardsInquisitor
            .QueryAsync<SetCardGroupIdResult>(query, partitionKey)
            .ConfigureAwait(false);

        if (response.IsNotSuccessful())
        {
            _dashboard.AddLog($"FAIL {setId}: query failed {response.StatusCode}");
            return map;
        }

        List<SetCardGroupIdResult> cards = response.Value.ToList();

        if (cards.Count > 0)
        {
            SetCardGroupIdResult firstCard = cards[0];
            string rawGroupId = firstCard.SetGroupId ?? "(null)";
            int nullCount = cards.Count(x => x.SetGroupId is null);

            // Write ALL set debug info to file
            string line = $"{setId}: {cards.Count} cards, grp={rawGroupId}, nulls={nullCount}";
            await System.IO.File.AppendAllTextAsync("sets-debug.txt", line + Environment.NewLine).ConfigureAwait(false);
            _dashboard.AddLog(line);
        }

        foreach (SetCardGroupIdResult card in cards)
        {
            map[card.Id] = card.SetGroupId ?? "core";
        }

        return map;
    }

    private async Task DebugQueryHomelandsAsync(string setId)
    {
        // Query to see what fields exist in the data object
        QueryDefinition debugQuery = new("SELECT TOP 1 c.id, c.data FROM c WHERE c.partition = @setId");
        debugQuery = debugQuery.WithParameter("@setId", setId);

        PartitionKey partitionKey = new(setId);
        OpResponse<IEnumerable<dynamic>> response = await _setCardsInquisitor
            .QueryAsync<dynamic>(debugQuery, partitionKey)
            .ConfigureAwait(false);

        if (response.IsSuccessful() && response.Value.Any())
        {
            dynamic firstDoc = response.Value.First();
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(firstDoc, Newtonsoft.Json.Formatting.Indented);

            // Write to file so we can actually see it
            string debugPath = $"hml-debug-{DateTime.UtcNow:yyyyMMdd-HHmmss}.json";
            await System.IO.File.WriteAllTextAsync(debugPath, json).ConfigureAwait(false);
            _dashboard.AddLog($"HML DEBUG written to: {debugPath}");
        }
    }

    private sealed class SetCardGroupIdResult
    {
        [Newtonsoft.Json.JsonProperty("id")]
        public string Id { get; init; } = string.Empty;

        [Newtonsoft.Json.JsonProperty("set_group_id")]
        public string SetGroupId { get; init; } = "core";
    }

    private async Task<ReconcileSetResult> ReconcileSetAsync(
        string userId,
        string setId,
        List<UserCardExtEntity> cardsInSet,
        UserSetCardExtEntity existingSetCard,
        Dictionary<string, string> cardIdToSetGroupId)
    {
        ReconcileSetResult result = new();

        Dictionary<string, UserSetCardGroupExtEntity> expectedGroups = BuildExpectedGroups(cardsInSet, cardIdToSetGroupId);

        int expectedTotalCards = cardsInSet.Sum(c => c.CollectedList?.Sum(cl => cl.Count) ?? 0);
        int expectedUniqueCards = cardsInSet.Count;

        if (existingSetCard is null)
        {
            result.HasDiscrepancy = true;
            result.CardsAdded = CountAllCardsInGroups(expectedGroups);

            if (_dryRun is false)
            {
                UserSetCardExtEntity newSetCard = new()
                {
                    UserId = userId,
                    SetId = setId,
                    TotalCards = expectedTotalCards,
                    UniqueCards = expectedUniqueCards,
                    Collecting = [],
                    Groups = expectedGroups
                };

                OpResponse<UserSetCardExtEntity> upsertResponse = await _userSetCardsScribe
                    .UpsertAsync(newSetCard)
                    .ConfigureAwait(false);

                result.Updated = upsertResponse.IsSuccessful();
                result.HasError = upsertResponse.IsNotSuccessful();
            }

            return result;
        }

        CompareGroupsResult comparison = CompareGroups(existingSetCard.Groups, expectedGroups);

        bool totalCardsMatch = existingSetCard.TotalCards == expectedTotalCards;
        bool uniqueCardsMatch = existingSetCard.UniqueCards == expectedUniqueCards;
        bool collectingCountsMatch = CheckCollectingCounts(existingSetCard.Collecting, expectedGroups);

        if (comparison.HasDifferences is false && totalCardsMatch && uniqueCardsMatch && collectingCountsMatch)
        {
            return result;
        }

        result.CollectingCountsMismatch = collectingCountsMatch is false;

        result.HasDiscrepancy = true;
        result.CardsAdded = comparison.CardsToAdd;
        result.CardsRemoved = comparison.CardsToRemove;
        result.TotalCardsMismatch = totalCardsMatch is false;
        result.UniqueCardsMismatch = uniqueCardsMatch is false;
        result.ExpectedTotalCards = expectedTotalCards;
        result.ActualTotalCards = existingSetCard.TotalCards;
        result.ExpectedUniqueCards = expectedUniqueCards;
        result.ActualUniqueCards = existingSetCard.UniqueCards;

        if (_dryRun is false)
        {
            List<UserSetCardCollectingExtEntity> updatedCollecting = BuildUpdatedCollecting(
                existingSetCard.Collecting,
                expectedGroups);

            UserSetCardExtEntity updatedSetCard = new()
            {
                UserId = userId,
                SetId = setId,
                TotalCards = expectedTotalCards,
                UniqueCards = expectedUniqueCards,
                Collecting = updatedCollecting,
                Groups = expectedGroups
            };

            // Debug: log what we're about to write
            string groupKeys = string.Join(",", expectedGroups.Keys);
            await System.IO.File.AppendAllTextAsync("reconciler-debug.txt", $"UPSERT: groups=[{groupKeys}] total={expectedTotalCards} unique={expectedUniqueCards}{Environment.NewLine}").ConfigureAwait(false);

            OpResponse<UserSetCardExtEntity> upsertResponse = await _userSetCardsScribe
                .UpsertAsync(updatedSetCard)
                .ConfigureAwait(false);

            // Debug: log upsert result
            await System.IO.File.AppendAllTextAsync("reconciler-debug.txt", $"UPSERT RESULT: {upsertResponse.StatusCode} success={upsertResponse.IsSuccessful()}{Environment.NewLine}").ConfigureAwait(false);

            result.Updated = upsertResponse.IsSuccessful();
            result.HasError = upsertResponse.IsNotSuccessful();
        }

        return result;
    }

    private static List<UserSetCardCollectingExtEntity> BuildUpdatedCollecting(
        ICollection<UserSetCardCollectingExtEntity> existingCollecting,
        Dictionary<string, UserSetCardGroupExtEntity> groups)
    {
        List<UserSetCardCollectingExtEntity> updated = [];
        HashSet<string> usedGroupIds = [];

        // If there's exactly one collecting entry and one group, they should match
        // (handles case where set_group_id changed from "booster" to "core" etc.)
        if (existingCollecting.Count == 1 && groups.Count == 1)
        {
            UserSetCardCollectingExtEntity existing = existingCollecting.First();
            KeyValuePair<string, UserSetCardGroupExtEntity> groupEntry = groups.First();

            // Use the actual group's ID, not the old collecting entry's ID
            updated.Add(BuildCollectingEntry(
                groupEntry.Key,
                groupEntry.Value,
                existing.Collecting,
                existing.CollectingFinishes));

            return updated;
        }

        // For multiple groups, try to match by set_group_id
        foreach (UserSetCardCollectingExtEntity existing in existingCollecting)
        {
            if (groups.TryGetValue(existing.SetGroupId, out UserSetCardGroupExtEntity group))
            {
                usedGroupIds.Add(existing.SetGroupId);
                updated.Add(BuildCollectingEntry(existing.SetGroupId, group, existing.Collecting, existing.CollectingFinishes));
            }
            else
            {
                // No cards in this group - check if this is the only collecting entry
                // and there's a group that wasn't matched (ID mismatch)
                List<string> unmatchedGroupIds = groups.Keys.Where(k => usedGroupIds.Contains(k) is false).ToList();

                if (existingCollecting.Count == 1 && unmatchedGroupIds.Count == 1)
                {
                    // Single mismatch - use the actual group's ID
                    string actualGroupId = unmatchedGroupIds[0];
                    usedGroupIds.Add(actualGroupId);
                    updated.Add(BuildCollectingEntry(actualGroupId, groups[actualGroupId], existing.Collecting, existing.CollectingFinishes));
                }
                else
                {
                    // Keep the entry with zero counts
                    updated.Add(new UserSetCardCollectingExtEntity
                    {
                        SetGroupId = existing.SetGroupId,
                        Collecting = existing.Collecting,
                        Count = 0,
                        Counts = new FinishCountsExtEntity
                        {
                            Total = 0,
                            NonFoil = 0,
                            Foil = 0,
                            Etched = 0
                        },
                        CollectingFinishes = existing.CollectingFinishes
                    });
                }
            }
        }

        // Add entries for any groups that don't have collecting entries yet
        foreach (KeyValuePair<string, UserSetCardGroupExtEntity> kvp in groups)
        {
            if (usedGroupIds.Contains(kvp.Key))
            {
                continue;
            }

            // New group with cards - create a collecting entry
            updated.Add(BuildCollectingEntry(kvp.Key, kvp.Value, collecting: true, collectingFinishes: ["nonFoil"]));
        }

        return updated;
    }

    private static UserSetCardCollectingExtEntity BuildCollectingEntry(
        string setGroupId,
        UserSetCardGroupExtEntity group,
        bool collecting,
        IReadOnlyCollection<string> collectingFinishes)
    {
        int nonFoilCount = group.NonFoil.Cards.Count;
        int foilCount = group.Foil.Cards.Count;
        int etchedCount = group.Etched.Cards.Count;

        HashSet<string> uniqueCards = [];

        foreach (string cardId in group.NonFoil.Cards) { uniqueCards.Add(cardId); }

        foreach (string cardId in group.Foil.Cards) { uniqueCards.Add(cardId); }

        foreach (string cardId in group.Etched.Cards) { uniqueCards.Add(cardId); }

        return new UserSetCardCollectingExtEntity
        {
            SetGroupId = setGroupId,
            Collecting = collecting,
            Count = uniqueCards.Count,
            Counts = new FinishCountsExtEntity
            {
                Total = uniqueCards.Count,
                NonFoil = nonFoilCount,
                Foil = foilCount,
                Etched = etchedCount
            },
            CollectingFinishes = collectingFinishes
        };
    }

    private Dictionary<string, UserSetCardGroupExtEntity> BuildExpectedGroups(
        List<UserCardExtEntity> cardsInSet,
        Dictionary<string, string> cardIdToSetGroupId)
    {
        Dictionary<string, UserSetCardGroupExtEntity> groups = [];

        int foundCount = 0;
        int missedCount = 0;

        // Get set code for file logging
        string setCode = cardsInSet.FirstOrDefault()?.SetCode ?? "unknown";

        // Debug: show first card ID from each side to check for format mismatch
        if (cardsInSet.Count > 0 && cardIdToSetGroupId.Count > 0)
        {
            string firstUserCardId = cardsInSet[0].CardId;
            string firstMapKey = cardIdToSetGroupId.Keys.First();
            string debugLine = $"{setCode}: UserCard={firstUserCardId} MapKey={firstMapKey} MapSize={cardIdToSetGroupId.Count}";
            _ = System.IO.File.AppendAllTextAsync("reconciler-debug.txt", debugLine + Environment.NewLine);
        }

        foreach (UserCardExtEntity card in cardsInSet)
        {
            if (cardIdToSetGroupId.TryGetValue(card.CardId, out string setGroupId) is false)
            {
                missedCount++;
                // Log to file
                string missLine = $"{setCode}: MISS CardId={card.CardId}";
                _ = System.IO.File.AppendAllTextAsync("reconciler-debug.txt", missLine + Environment.NewLine);

                continue; // Skip this card entirely - don't silently assign to "core"
            }

            foundCount++;

            if (groups.TryGetValue(setGroupId, out UserSetCardGroupExtEntity group) is false)
            {
                group = new UserSetCardGroupExtEntity
                {
                    NonFoil = new UserSetCardFinishGroupExtEntity { Cards = [] },
                    Foil = new UserSetCardFinishGroupExtEntity { Cards = [] },
                    Etched = new UserSetCardFinishGroupExtEntity { Cards = [] }
                };
                groups[setGroupId] = group;
            }

            if (card.CollectedList is null)
            {
                continue;
            }

            foreach (UserCardDetailsExtEntity collected in card.CollectedList)
            {
                if (collected.Count < 1)
                {
                    continue;
                }

                string finish = collected.Finish?.ToUpperInvariant() ?? "NONFOIL";

                ICollection<string> targetCards = finish switch
                {
                    "FOIL" => group.Foil.Cards,
                    "ETCHED" => group.Etched.Cards,
                    _ => group.NonFoil.Cards
                };

                if (targetCards.Contains(card.CardId) is false)
                {
                    targetCards.Add(card.CardId);
                }
            }
        }

        // Log summary to file
        string summaryLine = $"{setCode}: found={foundCount} missed={missedCount} groups=[{string.Join(",", groups.Keys)}]";
        _ = System.IO.File.AppendAllTextAsync("reconciler-debug.txt", summaryLine + Environment.NewLine);

        return groups;
    }

    private static CompareGroupsResult CompareGroups(
        Dictionary<string, UserSetCardGroupExtEntity> existing,
        Dictionary<string, UserSetCardGroupExtEntity> expected)
    {
        CompareGroupsResult result = new();

        HashSet<string> existingCardIds = GetAllCardIds(existing);
        HashSet<string> expectedCardIds = GetAllCardIds(expected);

        result.CardsToAdd = expectedCardIds.Except(existingCardIds).Count();
        result.CardsToRemove = existingCardIds.Except(expectedCardIds).Count();

        // Also check if group KEYS are different (e.g., "core" vs "booster")
        bool groupKeysMatch = existing.Keys.OrderBy(k => k).SequenceEqual(expected.Keys.OrderBy(k => k));

        result.HasDifferences = result.CardsToAdd > 0 || result.CardsToRemove > 0 || groupKeysMatch is false;

        return result;
    }

    private static HashSet<string> GetAllCardIds(Dictionary<string, UserSetCardGroupExtEntity> groups)
    {
        HashSet<string> cardIds = [];

        foreach (UserSetCardGroupExtEntity group in groups.Values)
        {
            foreach (string cardId in group.NonFoil.Cards)
            {
                cardIds.Add(cardId);
            }

            foreach (string cardId in group.Foil.Cards)
            {
                cardIds.Add(cardId);
            }

            foreach (string cardId in group.Etched.Cards)
            {
                cardIds.Add(cardId);
            }
        }

        return cardIds;
    }

    private static int CountAllCardsInGroups(Dictionary<string, UserSetCardGroupExtEntity> groups) =>
        GetAllCardIds(groups).Count;

    private static bool CheckCollectingCounts(
        ICollection<UserSetCardCollectingExtEntity> collecting,
        Dictionary<string, UserSetCardGroupExtEntity> groups)
    {
        foreach (UserSetCardCollectingExtEntity item in collecting)
        {
            if (groups.TryGetValue(item.SetGroupId, out UserSetCardGroupExtEntity group))
            {
                HashSet<string> uniqueCards = [];

                foreach (string cardId in group.NonFoil.Cards) { uniqueCards.Add(cardId); }

                foreach (string cardId in group.Foil.Cards) { uniqueCards.Add(cardId); }

                foreach (string cardId in group.Etched.Cards) { uniqueCards.Add(cardId); }

                int expectedCount = uniqueCards.Count;
                int expectedNonFoil = group.NonFoil.Cards.Count;
                int expectedFoil = group.Foil.Cards.Count;
                int expectedEtched = group.Etched.Cards.Count;

                if (item.Count != expectedCount ||
                    item.Counts?.Total != expectedCount ||
                    item.Counts?.NonFoil != expectedNonFoil ||
                    item.Counts?.Foil != expectedFoil ||
                    item.Counts?.Etched != expectedEtched)
                {
                    return false;
                }
            }
            else if (item.Count != 0)
            {
                return false;
            }
        }

        return true;
    }

    private static string BuildChangeDescription(ReconcileSetResult result)
    {
        List<string> parts = [];

        if (result.CardsAdded > 0 || result.CardsRemoved > 0)
        {
            parts.Add($"+{result.CardsAdded}/-{result.CardsRemoved} cards");
        }

        if (result.TotalCardsMismatch)
        {
            parts.Add($"total {result.ActualTotalCards}->{result.ExpectedTotalCards}");
        }

        if (result.UniqueCardsMismatch)
        {
            parts.Add($"unique {result.ActualUniqueCards}->{result.ExpectedUniqueCards}");
        }

        if (result.CollectingCountsMismatch)
        {
            parts.Add("collecting counts");
        }

        return parts.Count > 0 ? string.Join(", ", parts) : "groups rebuilt";
    }

    private sealed class ReconcileSetResult
    {
        public bool HasDiscrepancy { get; set; }
        public bool Updated { get; set; }
        public bool HasError { get; set; }
        public int CardsAdded { get; set; }
        public int CardsRemoved { get; set; }
        public bool TotalCardsMismatch { get; set; }
        public bool UniqueCardsMismatch { get; set; }
        public bool CollectingCountsMismatch { get; set; }
        public int ExpectedTotalCards { get; set; }
        public int ActualTotalCards { get; set; }
        public int ExpectedUniqueCards { get; set; }
        public int ActualUniqueCards { get; set; }
    }

    private sealed class CompareGroupsResult
    {
        public bool HasDifferences { get; set; }
        public int CardsToAdd { get; set; }
        public int CardsToRemove { get; set; }
    }
}
