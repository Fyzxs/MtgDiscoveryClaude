using Lib.Scryfall.Ingestion.Apis.Models;

namespace Lib.Scryfall.Ingestion.Apis.Filters;

/// <summary>
/// Filters out digital-only sets.
/// </summary>
internal sealed class NonDigitalSetFilter : IScryfallSetFilter
{
    public bool ShouldInclude(IScryfallSet set)
    {
        bool isDigital = set.Data().digital ?? false;
        return !isDigital;
    }
}