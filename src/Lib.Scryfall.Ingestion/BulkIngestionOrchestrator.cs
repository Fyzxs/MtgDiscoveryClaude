using System;
using System.Collections.Generic;
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
            Dictionary<string, IScryfallSet> sets = await _setsPipeline.ProcessSetsAsync().ConfigureAwait(false);

            Dictionary<string, IScryfallRuling> rulings = new();
            if (_config.ProcessRulings)
            {
                rulings = await _rulingsPipeline.ProcessRulingsAsync().ConfigureAwait(false);
            }

            int cardsCount = await _cardsPipeline.ProcessCardsAsync().ConfigureAwait(false);

            string completionMessage = _config.ProcessRulings
                ? $"Ingestion completed: {sets.Count} sets, {rulings.Count} rulings, {cardsCount} cards processed"
                : $"Ingestion completed: {sets.Count} sets, {cardsCount} cards processed (rulings skipped)";

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
}