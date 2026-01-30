using System.Collections.Generic;
using Lib.Scryfall.Ingestion.Configurations;
using Lib.Scryfall.Shared.Apis.Models;

namespace Lib.Scryfall.Ingestion.Filters;

internal sealed class SpecificSetsFilter : IScryfallSetFilter
{
    private const bool Include = true;
    private readonly IScryfallIngestionConfiguration _config;

    public SpecificSetsFilter(IScryfallIngestionConfiguration config) => _config = config;

    public bool ShouldInclude(IScryfallSet set)
    {
        SpecificSetCodes specificSetToProcess = _config.ProcessingConfig().SpecificSets();
        if (specificSetToProcess.HasNoSpecificSets()) return Include;

        ISet<string> asSystemType = specificSetToProcess.AsSystemType();
        return asSystemType.Contains(set.Code().ToLowerInvariant());
    }
}
