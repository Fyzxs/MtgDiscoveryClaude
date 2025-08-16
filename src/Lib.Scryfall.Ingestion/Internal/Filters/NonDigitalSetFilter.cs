using Lib.Scryfall.Ingestion.Apis.Filters;
using Lib.Scryfall.Ingestion.Apis.Models;

namespace Lib.Scryfall.Ingestion.Internal.Filters;

/// <summary>
/// Filters out digital-only sets.
/// </summary>
internal sealed class NonDigitalSetFilter : IScryfallSetFilter
{
    public bool ShouldInclude(IScryfallSet set)
    {
        return set.IsNotDigital();
    }
}
