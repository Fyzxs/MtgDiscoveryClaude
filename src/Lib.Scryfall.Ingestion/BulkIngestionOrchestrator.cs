using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.Scryfall.Ingestion.Apis;
using Lib.Scryfall.Ingestion.Apis.Configuration;
using Lib.Scryfall.Ingestion.Apis.Dashboard;
using Lib.Scryfall.Ingestion.Apis.Pipeline;
using Lib.Scryfall.Shared.Apis.Models;
using Microsoft.Extensions.Logging;

namespace Lib.Scryfall.Ingestion;

internal sealed class BulkIngestionOrchestrator : IBulkIngestionOrchestrator
{
    private readonly IIngestionDashboard _dashboard;
    private readonly ISetsPipelineService _setsPipeline;
    private readonly IRulingsPipelineService _rulingsPipeline;
    private readonly ICardsPipelineService _cardsPipeline;
    private readonly IBulkProcessingConfiguration _config;

    public BulkIngestionOrchestrator(
        IIngestionDashboard dashboard,
        ISetsPipelineService setsPipeline,
        IRulingsPipelineService rulingsPipeline,
        ICardsPipelineService cardsPipeline,
        IBulkProcessingConfiguration config)
    {
        _dashboard = dashboard;
        _setsPipeline = setsPipeline;
        _rulingsPipeline = rulingsPipeline;
        _cardsPipeline = cardsPipeline;
        _config = config;
    }

    public async Task OrchestrateBulkIngestionAsync()
    {
        _dashboard.SetStartTime();
        _dashboard.Refresh();

        try
        {
            // Phase 1: Fetch all data
            _dashboard.LogFetchingPhase();

            Dictionary<string, IScryfallSet> sets = await _setsPipeline.FetchSetsAsync().ConfigureAwait(false);

            Dictionary<string, IScryfallRuling> rulings = new();
            if (_config.ProcessRulings)
            {
                rulings = await _rulingsPipeline.FetchAndAggregateRulingsAsync().ConfigureAwait(false);
            }

            IReadOnlyList<dynamic> cards = await _cardsPipeline.FetchCardsAsync().ConfigureAwait(false);

            // Phase 2: Process cards (relationships, artists, etc.)
            _dashboard.LogProcessingPhase();

            // Convert sets to a lookup by set code
            Dictionary<string, dynamic> setsByCode = sets.ToDictionary(
                kvp => kvp.Value.Code(),
                kvp => kvp.Value.Data()
            );

            _cardsPipeline.ProcessCards(cards, setsByCode);

            // Phase 3: Write all data
            _dashboard.LogWritingPhase();

            await _setsPipeline.WriteSetsAsync(sets).ConfigureAwait(false);

            if (_config.ProcessRulings)
            {
                await _rulingsPipeline.WriteRulingsAsync(rulings).ConfigureAwait(false);
            }

            await _cardsPipeline.WriteCardsAsync(cards).ConfigureAwait(false);

            string completionMessage = _config.ProcessRulings
                ? $"Ingestion completed: {sets.Count} sets, {rulings.Count} rulings, {cards.Count} cards processed"
                : $"Ingestion completed: {sets.Count} sets, {cards.Count} cards processed (rulings skipped)";

            _dashboard.Complete(completionMessage);
        }
        catch (Exception ex)
        {
            _dashboard.LogBulkIngestionFailed(ex);
            _dashboard.Complete($"Ingestion failed: {ex.Message}");
            throw;
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