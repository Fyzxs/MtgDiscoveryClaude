using System.Collections.Generic;

namespace Lib.Shared.DataModels.Entities.Oufs.UserCards.Signing;

/// <summary>
/// Represents a set grouping in the signing results with aggregated statistics.
/// </summary>
public interface ISigningSetGroupOufEntity
{
    /// <summary>
    /// The unique identifier of the set.
    /// </summary>
    string SetId { get; }

    /// <summary>
    /// The code of the set (e.g., "DOM" for Dominaria).
    /// </summary>
    string SetCode { get; }

    /// <summary>
    /// The display name of the set.
    /// </summary>
    string SetName { get; }

    /// <summary>
    /// The number of selected convention artists with cards in this set.
    /// </summary>
    int ArtistCount { get; }

    /// <summary>
    /// The total number of unique cards with unsigned copies in this set.
    /// </summary>
    int UnsignedCardCount { get; }

    /// <summary>
    /// The release date of the set in ISO format (e.g., "1993-12-17").
    /// Used for sorting vintage sets (1996 and earlier).
    /// </summary>
    string ReleasedAt { get; }

    /// <summary>
    /// The artists from the convention selection that have cards in this set.
    /// </summary>
    IEnumerable<ISigningArtistGroupOufEntity> Artists { get; }
}
