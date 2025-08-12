using Lib.Scryfall.Ingestion.Apis.Values;

namespace Lib.Scryfall.Ingestion.Apis.Models;

/// <summary>
/// Interface for objects that have a Scryfall search URI.
/// </summary>
public interface IScryfallSearchUri
{
    /// <summary>
    /// Gets the search URI.
    /// </summary>
    Url SearchUri();
}