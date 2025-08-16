using Lib.Scryfall.Ingestion.Apis.Models;
using Lib.Scryfall.Ingestion.Internal.Filters;

namespace Lib.Scryfall.Ingestion.Internal.Filters;
internal sealed class NonDigitalSetFilter : IScryfallSetFilter
{
    public bool ShouldInclude(IScryfallSet set)
    {
        return set.IsNotDigital();
    }
}
