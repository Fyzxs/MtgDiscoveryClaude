using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.Entities;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators;
using Lib.Scryfall.Ingestion.Apis.Aggregation;
using Lib.Scryfall.Ingestion.Apis.Configuration;
using Lib.Scryfall.Ingestion.Apis.Dashboard;
using Lib.Scryfall.Ingestion.Apis.Pipeline;
using Lib.Scryfall.Ingestion.BulkIngestion;
using Lib.Scryfall.Shared.Apis.Models;
using Microsoft.Extensions.Logging;

namespace Lib.Scryfall.Ingestion.Pipeline;

internal sealed class RulingsPipelineService : IRulingsPipelineService
{
    private readonly RulingsBulkDataFetcher _rulingsFetcher;
    private readonly IRulingsAggregator _rulingsAggregator;
    private readonly ScryfallRulingItemsScribe _rulingScribe;
    private readonly IIngestionDashboard _dashboard;
    private readonly IBulkProcessingConfiguration _config;

    public RulingsPipelineService(
        RulingsBulkDataFetcher rulingsFetcher,
        IRulingsAggregator rulingsAggregator,
        ScryfallRulingItemsScribe rulingScribe,
        IIngestionDashboard dashboard,
        IBulkProcessingConfiguration config)
    {
        _rulingsFetcher = rulingsFetcher;
        _rulingsAggregator = rulingsAggregator;
        _rulingScribe = rulingScribe;
        _dashboard = dashboard;
        _config = config;
    }

    public async Task<Dictionary<string, IScryfallRuling>> ProcessRulingsAsync()
    {
        Dictionary<string, IScryfallRuling> rulings = await FetchAndAggregateRulingsAsync().ConfigureAwait(false);
        await WriteRulingsAsync(rulings).ConfigureAwait(false);
        return rulings;
    }

    private async Task<Dictionary<string, IScryfallRuling>> FetchAndAggregateRulingsAsync()
    {
        _dashboard.LogFetchingRulings();

        List<dynamic> allRulings = new();
        IAsyncEnumerable<dynamic> rulingStream = await _rulingsFetcher.FetchRulingsAsync().ConfigureAwait(false);
        int totalRulings = 0;

        await foreach (dynamic ruling in rulingStream.ConfigureAwait(false))
        {
            allRulings.Add(ruling);
            totalRulings++;

            string oracleId = ruling.oracle_id ?? string.Empty;
            _dashboard.UpdateRulingProgress(totalRulings, 0, $"Fetching ruling: {oracleId}");

            if (_config.EnableMemoryThrottling)
            {
                _dashboard.UpdateMemoryUsage();
            }

            if (totalRulings % _config.DashboardRefreshFrequency == 0)
            {
                _dashboard.Refresh();
            }
        }

        Dictionary<string, IScryfallRuling> aggregatedRulings = _rulingsAggregator.AggregateByOracleId(allRulings);
        _dashboard.LogRulingsFetched(aggregatedRulings.Count);
        return aggregatedRulings;
    }

    private async Task WriteRulingsAsync(Dictionary<string, IScryfallRuling> rulings)
    {
        _dashboard.LogWritingRulings(rulings.Count);

        int current = 0;
        int total = rulings.Count;

        foreach (KeyValuePair<string, IScryfallRuling> kvp in rulings)
        {
            current++;
            IScryfallRuling ruling = kvp.Value;

            _dashboard.UpdateRulingProgress(current, total, $"Writing ruling: {ruling.OracleId()}");

            if (current % _config.DashboardRefreshFrequency == 0)
            {
                _dashboard.Refresh();
            }

            ScryfallRulingItem entity = new()
            {
                Data = ruling.Data()
            };

            await _rulingScribe.UpsertAsync(entity).ConfigureAwait(false);
        }

        _dashboard.LogRulingsWritten(rulings.Count);
    }
}

internal static partial class RulingsPipelineServiceLoggerExtensions
{
    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Fetching rulings from Scryfall bulk data")]
    public static partial void LogFetchingRulings(this ILogger logger);

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Fetched {Count} unique rulings")]
    public static partial void LogRulingsFetched(this ILogger logger, int count);

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Writing {Count} rulings to Cosmos DB")]
    public static partial void LogWritingRulings(this ILogger logger, int count);

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Successfully wrote {Count} rulings to Cosmos DB")]
    public static partial void LogRulingsWritten(this ILogger logger, int count);
}