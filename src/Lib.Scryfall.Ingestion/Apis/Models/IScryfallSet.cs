using System.Collections.Generic;

namespace Lib.Scryfall.Ingestion.Apis.Models;

/// <summary>
/// Represents a Scryfall set.
/// </summary>
public interface IScryfallSet : IScryfallSearchUri
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
    /// Gets the raw data.
    /// </summary>
    dynamic Data();

    /// <summary>
    /// Gets all cards in this set.
    /// </summary>
    /// <returns>An asynchronous enumerable of cards in the set.</returns>
    IAsyncEnumerable<IScryfallCard> Cards();
}
