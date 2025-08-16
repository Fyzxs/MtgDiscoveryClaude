using Lib.Scryfall.Ingestion.Apis.Models;

namespace Lib.Scryfall.Ingestion.Internal.Filters;
internal interface IScryfallSetFilter
{
    bool ShouldInclude(IScryfallSet set);
    bool ShouldNotInclude(IScryfallSet set) => ShouldInclude(set) is false;
}
