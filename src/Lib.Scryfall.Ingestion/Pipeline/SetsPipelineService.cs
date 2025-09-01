using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.Entities;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators;
using Lib.Scryfall.Ingestion.Apis.Configuration;
using Lib.Scryfall.Ingestion.Apis.Dashboard;
using Lib.Scryfall.Ingestion.Apis.Pipeline;
using Lib.Scryfall.Shared.Apis.Models;
using Microsoft.Extensions.Logging;

namespace Lib.Scryfall.Ingestion.Pipeline;

internal sealed class SetsPipelineService : ISetsPipelineService
{
    private readonly IAsyncEnumerable<IScryfallSet> _sets;
    private readonly ScryfallSetItemsScribe _setScribe;
    private readonly IIngestionDashboard _dashboard;
    private readonly IBulkProcessingConfiguration _config;

    public SetsPipelineService(
        IAsyncEnumerable<IScryfallSet> sets,
        ScryfallSetItemsScribe setScribe,
        IIngestionDashboard dashboard,
        IBulkProcessingConfiguration config)
    {
        _sets = sets;
        _setScribe = setScribe;
        _dashboard = dashboard;
        _config = config;
    }

    public async Task<Dictionary<string, IScryfallSet>> FetchSetsAsync()
    {
        _dashboard.LogFetchingSets();

        Dictionary<string, IScryfallSet> sets = new();

        await foreach (IScryfallSet set in _sets.ConfigureAwait(false))
        {
            sets[set.Id()] = set;
            _dashboard.UpdateSetProgress(sets.Count, 0, set.Name());
        }

        _dashboard.LogSetsFetched(sets.Count);
        return sets;
    }

    public async Task WriteSetsAsync(Dictionary<string, IScryfallSet> sets)
    {
        _dashboard.LogWritingSets(sets.Count);

        int current = 0;
        int total = sets.Count;

        foreach (KeyValuePair<string, IScryfallSet> kvp in sets)
        {
            current++;
            IScryfallSet set = kvp.Value;

            _dashboard.UpdateSetProgress(current, total, $"Writing: {set.Name()}");

            ScryfallSetItem entity = new()
            {
                Data = set.Data()
            };

            await _setScribe.UpsertAsync(entity).ConfigureAwait(false);
            _dashboard.AddCompletedSet(set.Name());
        }

        _dashboard.LogSetsWritten(sets.Count);
    }
}

internal static partial class SetsPipelineServiceLoggerExtensions
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
}