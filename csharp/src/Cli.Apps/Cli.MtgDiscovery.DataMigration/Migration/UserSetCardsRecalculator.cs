using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserCards;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserSetCards;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Gophers;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitors;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Scribes;
using Lib.Cosmos.Apis.Ids;
using Lib.Cosmos.Apis.Operators;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;

namespace Cli.MtgDiscovery.DataMigration.Migration;

internal sealed class UserSetCardsRecalculator
{
    private readonly ILogger _logger;
    private readonly UserCardsInquisitor _userCardsInquisitor;
    private readonly UserSetCardsGopher _userSetCardsGopher;
    private readonly UserSetCardsScribe _userSetCardsScribe;

    public UserSetCardsRecalculator(ILogger logger)
    {
        _logger = logger;
        _userCardsInquisitor = new UserCardsInquisitor(logger);
        _userSetCardsGopher = new UserSetCardsGopher(logger);
        _userSetCardsScribe = new UserSetCardsScribe(logger);
    }

    public async Task RecalculateAsync(string userId)
    {
        _logger.LogInformation("Starting UserSetCards recalculation for user {UserId}", userId);

        // Query all UserCards for this user
        QueryDefinition query = new("SELECT * FROM c WHERE c.partition = @userId");
        query = query.WithParameter("@userId", userId);

        PartitionKey partitionKey = new(userId);
        OpResponse<IEnumerable<UserCardExtEntity>> response = await _userCardsInquisitor
            .QueryAsync<UserCardExtEntity>(query, partitionKey)
            .ConfigureAwait(false);

        if (response.IsNotSuccessful())
        {
            _logger.LogError("Failed to query UserCards for recalculation: {StatusCode}", response.StatusCode);
            return;
        }

        List<UserCardExtEntity> userCards = [.. response.Value];
        _logger.LogInformation("Found {Count} UserCards to process", userCards.Count);

        // Group by SetId and calculate totals
        Dictionary<string, SetAggregation> setAggregations = [];

        foreach (UserCardExtEntity card in userCards)
        {
            if (setAggregations.TryGetValue(card.SetId, out SetAggregation existing) is false)
            {
                existing = new SetAggregation
                {
                    SetId = card.SetId,
                    UserId = userId
                };
                setAggregations[card.SetId] = existing;
            }

            // Count unique cards
            existing.UniqueCards++;

            // Sum total cards from all collected versions
            int cardTotal = card.CollectedList?.Sum(c => c.Count) ?? 0;
            existing.TotalCards += cardTotal;
        }

        _logger.LogInformation("Calculated totals for {Count} sets", setAggregations.Count);

        // Upsert UserSetCards for each set, preserving Collecting and Groups
        int successCount = 0;
        int errorCount = 0;

        foreach (SetAggregation aggregation in setAggregations.Values)
        {
            // Read existing UserSetCard to preserve Collecting and Groups
            ReadPointItem readPointItem = new()
            {
                Id = new ProvidedCosmosItemId(aggregation.SetId),
                Partition = new ProvidedPartitionKeyValue(aggregation.UserId)
            };
            OpResponse<UserSetCardExtEntity> existingResponse = await _userSetCardsGopher
                .ReadAsync<UserSetCardExtEntity>(readPointItem)
                .ConfigureAwait(false);

            UserSetCardExtEntity userSetCard;
            if (existingResponse.IsSuccessful())
            {
                // Preserve existing Collecting and Groups, only update totals
                UserSetCardExtEntity existing = existingResponse.Value;
                userSetCard = new UserSetCardExtEntity
                {
                    UserId = aggregation.UserId,
                    SetId = aggregation.SetId,
                    TotalCards = aggregation.TotalCards,
                    UniqueCards = aggregation.UniqueCards,
                    Collecting = existing.Collecting,
                    Groups = existing.Groups
                };
            }
            else
            {
                // No existing record, create new with empty collections
                userSetCard = new UserSetCardExtEntity
                {
                    UserId = aggregation.UserId,
                    SetId = aggregation.SetId,
                    TotalCards = aggregation.TotalCards,
                    UniqueCards = aggregation.UniqueCards,
                    Collecting = [],
                    Groups = []
                };
            }

            OpResponse<UserSetCardExtEntity> upsertResponse = await _userSetCardsScribe
                .UpsertAsync(userSetCard)
                .ConfigureAwait(false);

            if (upsertResponse.IsSuccessful())
            {
                successCount++;
            }
            else
            {
                errorCount++;
                _logger.LogError("Failed to upsert UserSetCard for set {SetId}: {StatusCode}",
                    aggregation.SetId, upsertResponse.StatusCode);
            }
        }

        _logger.LogInformation("UserSetCards recalculation complete. Success: {Success}, Errors: {Errors}",
            successCount, errorCount);
    }

    private sealed class SetAggregation
    {
        public string SetId { get; init; }
        public string UserId { get; init; }
        public int TotalCards { get; set; }
        public int UniqueCards { get; set; }
    }
}
