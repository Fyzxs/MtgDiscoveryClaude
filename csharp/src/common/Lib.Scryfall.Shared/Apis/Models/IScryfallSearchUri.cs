using Lib.Universal.Primitives;

namespace Lib.Scryfall.Shared.Apis.Models;

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
