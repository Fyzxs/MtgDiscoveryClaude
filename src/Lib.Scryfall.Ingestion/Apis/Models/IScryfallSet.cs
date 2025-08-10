using Lib.Scryfall.Ingestion.Apis.Values;

namespace Lib.Scryfall.Ingestion.Apis.Models;

/// <summary>
/// Represents a Scryfall set.
/// </summary>
public interface IScryfallSet
{
    /// <summary>
    /// Gets the set code.
    /// </summary>
    string Code();

    /// <summary>
    /// Gets the set name.
    /// </summary>
    string Name();

    /// <summary>
    /// Gets the URL for fetching this set's cards.
    /// </summary>
    Url SearchUri();

    /// <summary>
    /// Gets the raw data.
    /// </summary>
    dynamic Data();
}
