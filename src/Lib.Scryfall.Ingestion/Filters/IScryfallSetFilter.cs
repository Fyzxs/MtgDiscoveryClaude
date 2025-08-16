using Lib.Scryfall.Shared.Apis.Models;

namespace Lib.Scryfall.Ingestion.Filters;
internal interface IScryfallSetFilter
{
    bool ShouldInclude(IScryfallSet set);
    bool ShouldNotInclude(IScryfallSet set) => ShouldInclude(set) is false;
}
