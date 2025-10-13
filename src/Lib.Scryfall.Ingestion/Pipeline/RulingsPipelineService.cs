using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Scribes;
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

    public async Task<Dictionary<string, IScryfallRuling>> FetchAndAggregateRulingsAsync()
    {
        _dashboard.LogFetchingRulings();

        List<dynamic> allRulings = [];
        IAsyncEnumerable<dynamic> rulingStream = await _rulingsFetcher.FetchRulingsAsync().ConfigureAwait(false);
        int totalRulings = 0;

        await foreach (dynamic ruling in rulingStream.ConfigureAwait(false))
        {
            allRulings.Add(ruling);
            totalRulings++;

            string oracleId = ruling.oracle_id ?? string.Empty;
            _dashboard.UpdateProgress("Rulings:", totalRulings, 0, "Fetching", oracleId);
        }

        Dictionary<string, IScryfallRuling> aggregatedRulings = _rulingsAggregator.AggregateByOracleId(allRulings);
        _dashboard.LogRulingsFetched(aggregatedRulings.Count);
        return aggregatedRulings;
    }

    public async Task WriteRulingsAsync(IEnumerable<IScryfallRuling> rulings)
    {
        List<IScryfallRuling> rulingList = rulings.ToList();
        int total = rulingList.Count;

        _dashboard.LogWritingRulings(total);

        int current = 0;

        foreach (IScryfallRuling ruling in rulingList)
        {
            current++;

            _dashboard.UpdateProgress("Rulings:", current, total, "Writing", ruling.OracleId);

            ScryfallRulingExtEntity entity = new()
            {
                OracleId = ruling.OracleId,
                Data = ruling.Rulings
            };

            await _rulingScribe.UpsertAsync(entity).ConfigureAwait(false);
        }

        _dashboard.LogRulingsWritten(total);
        _dashboard.UpdateCompletedCount("Rulings", total);
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
