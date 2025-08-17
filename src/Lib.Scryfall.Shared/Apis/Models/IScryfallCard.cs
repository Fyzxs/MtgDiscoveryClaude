using System.Collections.Generic;

namespace Lib.Scryfall.Shared.Apis.Models;

/// <summary>
/// Represents a Scryfall card.
/// </summary>
public interface IScryfallCard
{
    /// <summary>
    /// Gets the card ID.
    /// </summary>
    string Id();

    /// <summary>
    /// Gets the card name.
    /// </summary>
    string Name();

    /// <summary>
    /// Gets the raw data.
    /// </summary>
    dynamic Data();

    IScryfallSet Set();

    /// <summary>
    /// Gets the image URIs for the card faces.
    /// </summary>
    ICardImageInfoCollection ImageUris();

    /// <summary>
    /// Gets the artist IDs for the card.
    /// </summary>
    IEnumerable<string> ArtistIds();

    /// <summary>
    /// Gets the artist ID and name pairs for the card.
    /// </summary>
    IEnumerable<IArtistIdNamePair> ArtistIdNamePairs();
}
