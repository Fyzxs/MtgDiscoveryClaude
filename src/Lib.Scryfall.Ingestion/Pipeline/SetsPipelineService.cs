using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Scribes;
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
    private readonly ScryfallSetAssociationsScribe _setParentAssociationsScribe;
    private readonly ScryfallSetCodeIndexScribe _setCodeIndexScribe;
    private readonly IIngestionDashboard _dashboard;
    private readonly IBulkProcessingConfiguration _config;
    private readonly ILogger _logger;

    public SetsPipelineService(
        IAsyncEnumerable<IScryfallSet> sets,
        ScryfallSetItemsScribe setScribe,
        IIngestionDashboard dashboard,
        IBulkProcessingConfiguration config)
    {
        _sets = sets;
        _setScribe = setScribe;
        _setParentAssociationsScribe = new ScryfallSetAssociationsScribe(dashboard);
        _setCodeIndexScribe = new ScryfallSetCodeIndexScribe(dashboard);
        _dashboard = dashboard;
        _config = config;
        _logger = dashboard;
    }

    public async Task<Dictionary<string, IScryfallSet>> FetchSetsAsync()
    {
        _dashboard.LogFetchingSets();

        Dictionary<string, IScryfallSet> sets = [];
        bool hasSetFilter = _config.SetCodesToProcess.Count > 0;
        System.DateTime? releasedAfter = _config.SetsReleasedAfter;
        int skippedCount = 0;

        await foreach (IScryfallSet set in _sets.ConfigureAwait(false))
        {
            // Skip sets not in the filter list if filtering is enabled
            if (hasSetFilter && _config.SetCodesToProcess.Contains(set.Code()) is false)
            {
                skippedCount++;
                _dashboard.LogSetSkipped(set.Code(), set.Name());
                continue;
            }

            // Skip sets released before the date filter if enabled
            if (releasedAfter.HasValue)
            {
                System.DateTime setReleaseDate = GetSetReleaseDate(set);
                if (setReleaseDate < releasedAfter.Value)
                {
                    skippedCount++;
                    _dashboard.LogSetSkipped(set.Code(), set.Name());
                    continue;
                }
            }

            sets[set.Id()] = set;
            _dashboard.UpdateProgress("Sets:", sets.Count, 0, "Fetching", set.Name());
        }

        if (skippedCount > 0)
        {
            _dashboard.LogSetsSkipped(skippedCount);
        }

        _dashboard.LogSetsFetched(sets.Count);
        return sets;
    }

    private static System.DateTime GetSetReleaseDate(IScryfallSet set)
    {
        try
        {
            dynamic data = set.Data();
            string releasedAt = data.released_at;
            if (string.IsNullOrEmpty(releasedAt) is false && System.DateTime.TryParse(releasedAt, out System.DateTime parsedDate))
            {
                return parsedDate;
            }
        }
        catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException)
        {
            return System.DateTime.MinValue;
        }

        return System.DateTime.MinValue;
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

            _dashboard.UpdateProgress("Sets:", current, total, "Writing", set.Name());

            ScryfallSetItemExtEntity entity = new()
            {
                Data = set.Data()
            };

            _ = await _setScribe.UpsertAsync(entity).ConfigureAwait(false);

            // Write SetCodeIndex (mapping set code to set ID)
            ScryfallSetCodeIndexExtEntity codeIndex = new()
            {
                SetCode = set.Code(),
                SetId = set.Id()
            };
            _ = await _setCodeIndexScribe.UpsertAsync(codeIndex).ConfigureAwait(false);

            // Write SetAssociation if this set has a parent
            if (set.HasParentSet())
            {
                ScryfallSetParentAssociationExtEntity parentAssociationItem = new()
                {
                    SetId = set.Id(),
                    ParentSetCode = set.ParentSetCode(),
                    SetCode = set.Code(),
                    SetName = set.Name()
                };
                _ = await _setParentAssociationsScribe.UpsertAsync(parentAssociationItem).ConfigureAwait(false);
            }

            _dashboard.AddCompletedSet(set.Name());
        }

        _dashboard.LogSetsWritten(sets.Count);
        _dashboard.UpdateCompletedCount("Sets", sets.Count);
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

    [LoggerMessage(
        Level = LogLevel.Debug,
        Message = "Skipping set {Code} ({Name}) - not in filter list")]
    public static partial void LogSetSkipped(this ILogger logger, string code, string name);

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Skipped {Count} sets due to filter configuration")]
    public static partial void LogSetsSkipped(this ILogger logger, int count);
}
