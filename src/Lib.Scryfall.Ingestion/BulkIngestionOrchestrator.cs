using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Scryfall.Ingestion.Apis;
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

    public BulkIngestionOrchestrator(
        IIngestionDashboard dashboard,
        ISetsPipelineService setsPipeline,
        IRulingsPipelineService rulingsPipeline)
    {
        _dashboard = dashboard;
        _setsPipeline = setsPipeline;
        _rulingsPipeline = rulingsPipeline;
    }

    public async Task OrchestrateBulkIngestionAsync()
    {
        _dashboard.SetStartTime();
        _dashboard.Refresh();

        try
        {
            Dictionary<string, IScryfallSet> sets = await _setsPipeline.ProcessSetsAsync().ConfigureAwait(false);
            Dictionary<string, IScryfallRuling> rulings = await _rulingsPipeline.ProcessRulingsAsync().ConfigureAwait(false);

            _dashboard.Complete($"Ingestion completed: {sets.Count} sets, {rulings.Count} rulings processed");
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