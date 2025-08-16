using Lib.Scryfall.Ingestion.Apis.Models;

namespace Lib.Scryfall.Ingestion.Apis.Filters;

/// <summary>
/// Defines a filter for Scryfall sets.
/// </summary>
public interface IScryfallSetFilter
{
    /// <summary>
    /// Determines whether a set should be included.
    /// </summary>
    bool ShouldInclude(IScryfallSet set);

    /// <summary>
    /// Determines whether a set should be included.
    /// </summary>
    bool ShouldNotInclude(IScryfallSet set) => ShouldInclude(set) is false;
}
