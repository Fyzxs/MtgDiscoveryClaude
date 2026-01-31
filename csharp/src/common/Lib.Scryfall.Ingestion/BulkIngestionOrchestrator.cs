using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Lib.Scryfall.Ingestion.Apis;
using Lib.Scryfall.Ingestion.Apis.Configuration;
using Lib.Scryfall.Ingestion.Apis.Dashboard;
using Lib.Scryfall.Ingestion.Apis.Pipeline;
using Lib.Scryfall.Ingestion.Services;
using Lib.Scryfall.Shared.Apis.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace Lib.Scryfall.Ingestion;

internal sealed class BulkIngestionOrchestrator : IBulkIngestionOrchestrator
{
    private readonly IIngestionDashboard _dashboard;
    private readonly ISetsPipelineService _setsPipeline;
    private readonly IRulingsPipelineService _rulingsPipeline;
    private readonly ICardsPipelineService _cardsPipeline;
    private readonly IArtistsPipelineService _artistsPipeline;
    private readonly ITrigramsPipelineService _trigramsPipeline;
    private readonly IBulkProcessingConfiguration _config;
    private readonly ICardGroupingMatcher _groupingMatcher;
    private readonly ISetGroupingsLoader _groupingsLoader;

    public BulkIngestionOrchestrator(
        IIngestionDashboard dashboard,
        ISetsPipelineService setsPipeline,
        IRulingsPipelineService rulingsPipeline,
        ICardsPipelineService cardsPipeline,
        IArtistsPipelineService artistsPipeline,
        ITrigramsPipelineService trigramsPipeline,
        IBulkProcessingConfiguration config)
    {
        _dashboard = dashboard;
        _setsPipeline = setsPipeline;
        _rulingsPipeline = rulingsPipeline;
        _cardsPipeline = cardsPipeline;
        _artistsPipeline = artistsPipeline;
        _trigramsPipeline = trigramsPipeline;
        _config = config;
        _groupingsLoader = new MonoStateSetGroupingsLoader();
        _groupingMatcher = new CardGroupingMatcher();
    }

    public async Task OrchestrateBulkIngestionAsync()
    {
        _dashboard.SetStartTime();
        _dashboard.Refresh();

        CancellationToken cancellationToken = _dashboard.GetCancellationToken();

        try
        {
            // Phase 1: Fetch all data
            _dashboard.LogFetchingPhase();

            Dictionary<string, IScryfallSet> sets = await _setsPipeline.FetchSetsAsync().ConfigureAwait(false);

            cancellationToken.ThrowIfCancellationRequested();

            Dictionary<string, IScryfallRuling> rulings = [];
            if (_config.ProcessRulings)
            {
                rulings = await _rulingsPipeline.FetchAndAggregateRulingsAsync().ConfigureAwait(false);
                cancellationToken.ThrowIfCancellationRequested();
            }

            IReadOnlyList<IScryfallCard> cards = [];
            if (_config.SetsOnly is false)
            {
                cards = await _cardsPipeline.FetchCardsAsync().ConfigureAwait(false);
                cancellationToken.ThrowIfCancellationRequested();

                // Phase 2: Process cards (relationships, artists, etc.)
                _dashboard.LogProcessingPhase();

                // Convert sets to a lookup by set code
                Dictionary<string, dynamic> setsByCode = sets.ToDictionary(
                    kvp => kvp.Value.Code(),
                    kvp => kvp.Value.Data()
                );

                _cardsPipeline.ProcessCards(cards, setsByCode);

                // Track artists, trigrams, and finish counts from cards
                foreach (IScryfallCard card in cards)
                {
                    _artistsPipeline.TrackArtist(card);
                    _trigramsPipeline.TrackCard(card);

                    // Track finish counts for set groupings
                    string setCode = card.Set().Code();
                    CardGrouping grouping = _groupingMatcher.GetGroupingForCard(card.Data(), setCode);
                    if (grouping is not null)
                    {
                        TrackFinishCounts(card.Data(), grouping);
                    }
                }

                cancellationToken.ThrowIfCancellationRequested();

                // Attach groupings to sets before writing
                AttachGroupingsToSets(sets);
            }

            // Phase 3: Write all data
            _dashboard.LogWritingPhase();

            await _setsPipeline.WriteSetsAsync(sets).ConfigureAwait(false);
            cancellationToken.ThrowIfCancellationRequested();

            if (_config.ProcessRulings)
            {
                await _rulingsPipeline.WriteRulingsAsync(rulings.Values).ConfigureAwait(false);
                cancellationToken.ThrowIfCancellationRequested();
            }

            if (_config.SetsOnly is false)
            {
                await _cardsPipeline.WriteCardsAsync(cards).ConfigureAwait(false);
                cancellationToken.ThrowIfCancellationRequested();

                await _artistsPipeline.WriteArtistsAsync().ConfigureAwait(false);
                cancellationToken.ThrowIfCancellationRequested();

                await _trigramsPipeline.WriteTrigramsAsync().ConfigureAwait(false);
                cancellationToken.ThrowIfCancellationRequested();
            }

            string completionMessage = _config.SetsOnly
                ? $"Sets-only ingestion completed: {sets.Count} sets processed"
                : _config.ProcessRulings
                    ? $"Ingestion completed: {sets.Count} sets, {rulings.Count} rulings, {cards.Count} cards processed"
                    : $"Ingestion completed: {sets.Count} sets, {cards.Count} cards processed (rulings skipped)";

            _dashboard.Complete(completionMessage);
        }
        catch (OperationCanceledException)
        {
            _dashboard.LogBulkIngestionCancelled();
            _dashboard.Complete("Ingestion cancelled by user");
        }
        catch (Exception ex)
        {
            _dashboard.LogBulkIngestionFailed(ex);
            _dashboard.Complete($"Ingestion failed: {ex.Message}");
            throw;
        }
    }

    private void AttachGroupingsToSets(Dictionary<string, IScryfallSet> sets)
    {
        foreach (KeyValuePair<string, IScryfallSet> kvp in sets)
        {
            IScryfallSet set = kvp.Value;
            string setCode = set.Code();

            SetGroupingData groupingData = _groupingsLoader.GetGroupingsForSet(setCode);
            if (groupingData is null || groupingData.Groupings is null || groupingData.Groupings.Count == 0)
            {
                continue;
            }

            dynamic setData = set.Data();
            if (setData is JObject setJObj)
            {
                JArray groupingsArray = [];
                foreach (CardGrouping grouping in groupingData.Groupings)
                {
                    JObject groupingObj = new()
                    {
                        ["id"] = grouping.Id,
                        ["displayName"] = grouping.DisplayName,
                        ["order"] = grouping.Order,
                        ["rawQuery"] = grouping.RawQuery
                    };

                    if (grouping.CardCounts is not null)
                    {
                        groupingObj["cardCounts"] = new JObject
                        {
                            ["total"] = grouping.CardCounts.Total,
                            ["nonFoil"] = grouping.CardCounts.NonFoil,
                            ["foil"] = grouping.CardCounts.Foil,
                            ["etched"] = grouping.CardCounts.Etched
                        };
                    }

                    groupingsArray.Add(groupingObj);
                }

                setJObj["groupings"] = groupingsArray;
            }
        }
    }

    private static void TrackFinishCounts(dynamic cardData, CardGrouping grouping)
    {
        grouping.CardCounts ??= new CardCounts();

        grouping.CardCounts.Total++;

        if (cardData is JObject jobj && jobj.TryGetValue("finishes", out JToken finishesToken) && finishesToken is JArray finishesArray)
        {
            foreach (JToken finishToken in finishesArray)
            {
                string finish = finishToken.ToString();
                if (string.Equals(finish, "nonfoil", StringComparison.OrdinalIgnoreCase))
                {
                    grouping.CardCounts.NonFoil++;
                }
                else if (string.Equals(finish, "foil", StringComparison.OrdinalIgnoreCase))
                {
                    grouping.CardCounts.Foil++;
                }
                else if (string.Equals(finish, "etched", StringComparison.OrdinalIgnoreCase))
                {
                    grouping.CardCounts.Etched++;
                }
            }
        }
    }
}

internal static partial class BulkIngestionOrchestratorLoggerExtensions
{
    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "Bulk ingestion failed")]
    public static partial void LogBulkIngestionFailed(this ILogger logger, Exception ex);

    [LoggerMessage(
        Level = LogLevel.Warning,
        Message = "Bulk ingestion cancelled by user")]
    public static partial void LogBulkIngestionCancelled(this ILogger logger);

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Starting FETCH phase - downloading all data from Scryfall")]
    public static partial void LogFetchingPhase(this ILogger logger);

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Starting PROCESS phase - building relationships and extracting data")]
    public static partial void LogProcessingPhase(this ILogger logger);

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Starting WRITE phase - persisting all data to storage")]
    public static partial void LogWritingPhase(this ILogger logger);
}
