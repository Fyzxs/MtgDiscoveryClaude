using System.Collections.Generic;
using Lib.Universal.Primitives;

namespace Lib.Scryfall.Shared.Apis.Models;

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
    /// Gets the unique identifier for this set.
    /// </summary>
    string Id();

    /// <summary>
    /// Gets whether this is a digital-only set.
    /// </summary>
    bool IsDigital();

    /// <summary>
    /// Gets whether this is not a digital-only set.
    /// </summary>
    bool IsNotDigital();

    /// <summary>
    /// Gets the icon SVG path.
    /// </summary>
    Url IconSvgPath();

    /// <summary>
    /// Gets the parent set code if this is a child set.
    /// </summary>
    string ParentSetCode();

    /// <summary>
    /// Gets whether this set has a parent set.
    /// </summary>
    bool HasParentSet();

    /// <summary>
    /// Gets all cards in this set.
    /// </summary>
    /// <returns>An asynchronous enumerable of cards in the set.</returns>
    IAsyncEnumerable<IScryfallCard> Cards();
}
