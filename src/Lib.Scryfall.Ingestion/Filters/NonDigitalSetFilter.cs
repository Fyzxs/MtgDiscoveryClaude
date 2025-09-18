using Lib.Scryfall.Shared.Apis.Models;

namespace Lib.Scryfall.Ingestion.Filters;

internal sealed class NonDigitalSetFilter : IScryfallSetFilter
{
    public bool ShouldInclude(IScryfallSet set)
    {
        return set.IsNotDigital();
    }
}
