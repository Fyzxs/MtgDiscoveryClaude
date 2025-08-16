using Lib.Scryfall.Ingestion.Apis.Configurations;
using Lib.Scryfall.Ingestion.Apis.Filters;
using Lib.Scryfall.Ingestion.Apis.Models;
using Lib.Scryfall.Ingestion.Configurations;

namespace Lib.Scryfall.Ingestion.Internal.Filters;

internal sealed class MaxSetsFilter : IScryfallSetFilter
{
    private const bool Exclude = false;
    private const bool Include = true;

    private readonly IScryfallIngestionConfiguration _config;
    private int _processedCount;

    public MaxSetsFilter(IScryfallIngestionConfiguration config)
    {
        _config = config;
    }

    public bool ShouldInclude(IScryfallSet set)
    {
        MaxSetsToProcess maxSetsToProcess = _config.ProcessingConfig().MaxSets();
        if (maxSetsToProcess.IsUnlimited()) return Include;

        if (maxSetsToProcess <= _processedCount) return Exclude;

        _processedCount++;
        return Include;
    }
}
