using System.Collections.Generic;
using Lib.Scryfall.Ingestion.Apis.Models;
using Lib.Scryfall.Ingestion.Configurations;
using Lib.Scryfall.Ingestion.Internal.Configurations;
using Lib.Scryfall.Ingestion.Internal.Filters;

namespace Lib.Scryfall.Ingestion.Internal.Filters;

internal sealed class SpecificSetsFilter : IScryfallSetFilter
{
    private const bool Include = true;
    private readonly IScryfallIngestionConfiguration _config;

    public SpecificSetsFilter(IScryfallIngestionConfiguration config)
    {
        _config = config;
    }

    public bool ShouldInclude(IScryfallSet set)
    {
        SpecificSetCodes specificSetToProcess = _config.ProcessingConfig().SpecificSets();
        if (specificSetToProcess.HasNoSpecificSets()) return Include;

        ISet<string> asSystemType = specificSetToProcess.AsSystemType();
        return asSystemType.Contains(set.Code().ToLowerInvariant());
    }
}
