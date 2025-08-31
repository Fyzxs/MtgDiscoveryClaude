using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.Entities;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators;
using Lib.Scryfall.Ingestion.Apis;
using Lib.Scryfall.Ingestion.Apis.Collections;
using Lib.Scryfall.Ingestion.Apis.Dashboard;
using Lib.Scryfall.Shared.Apis.Models;
using Microsoft.Extensions.Logging;

namespace Lib.Scryfall.Ingestion.BulkIngestion;

public sealed class BulkIngestionService : IBulkIngestionService
{
    private readonly IIngestionDashboard _dashboard;
    private readonly IAsyncEnumerable<IScryfallSet> _sets;
    private readonly ScryfallSetItemsScribe _setScribe;

    public BulkIngestionService(IIngestionDashboard dashboard, ILogger logger)
        : this(
            dashboard,
            new FilteredScryfallSetCollection(logger),
            new ScryfallSetItemsScribe(logger))
    {
    }

    private BulkIngestionService(
        IIngestionDashboard dashboard,
        IAsyncEnumerable<IScryfallSet> sets,
        ScryfallSetItemsScribe setScribe)
    {
        _dashboard = dashboard;
        _sets = sets;
        _setScribe = setScribe;
    }

    public async Task IngestBulkDataAsync()
    {
        _dashboard.SetStartTime();
        _dashboard.Refresh();

        try
        {
            // TB.1: Fetch sets from API and write to SetItems
            Dictionary<string, IScryfallSet> sets = await FetchSetsAsync().ConfigureAwait(false);
            await WriteSetsAsync(sets).ConfigureAwait(false);

            _dashboard.Complete($"Ingestion completed: {sets.Count} sets processed");
        }
        catch (Exception ex)
        {
            _dashboard.LogBulkIngestionFailed(ex);
            _dashboard.Complete($"Ingestion failed: {ex.Message}");
            throw;
        }
    }

    private async Task<Dictionary<string, IScryfallSet>> FetchSetsAsync()
    {
        _dashboard.LogFetchingSets();

        Dictionary<string, IScryfallSet> sets = new();

        await foreach (IScryfallSet set in _sets.ConfigureAwait(false))
        {
            sets[set.Id()] = set;
            _dashboard.UpdateSetProgress(sets.Count, 0, set.Name());
            _dashboard.UpdateMemoryUsage();
            _dashboard.Refresh();
        }

        _dashboard.LogSetsFetched(sets.Count);
        return sets;
    }

    private async Task WriteSetsAsync(Dictionary<string, IScryfallSet> sets)
    {
        _dashboard.LogWritingSets(sets.Count);

        int current = 0;
        int total = sets.Count;

        foreach (KeyValuePair<string, IScryfallSet> kvp in sets)
        {
            current++;
            IScryfallSet set = kvp.Value;

            _dashboard.UpdateSetProgress(current, total, $"Writing: {set.Name()}");
            _dashboard.Refresh();

            ScryfallSetItem entity = new()
            {
                Data = set.Data()
            };

            await _setScribe.UpsertAsync(entity).ConfigureAwait(false);
            _dashboard.AddCompletedSet(set.Name());
            _dashboard.Refresh();
        }

        _dashboard.LogSetsWritten(sets.Count);
    }
}

internal static partial class BulkIngestionServiceLoggerExtensions
{
    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Fetching sets from Scryfall API")]
    public static partial void LogFetchingSets(this ILogger logger);

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Fetched {Count} sets (after filtering)")]
    public static partial void LogSetsFetched(this ILogger logger, int count);

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Writing {Count} sets to Cosmos DB")]
    public static partial void LogWritingSets(this ILogger logger, int count);

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Successfully wrote {Count} sets to Cosmos DB")]
    public static partial void LogSetsWritten(this ILogger logger, int count);

    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "Bulk ingestion failed")]
    public static partial void LogBulkIngestionFailed(this ILogger logger, Exception ex);
}