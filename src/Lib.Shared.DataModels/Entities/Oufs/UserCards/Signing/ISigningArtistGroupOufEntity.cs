using System.Collections.Generic;

namespace Lib.Shared.DataModels.Entities.Oufs.UserCards.Signing;

/// <summary>
/// Represents an artist grouping within a set for signing results.
/// </summary>
public interface ISigningArtistGroupOufEntity
{
    /// <summary>
    /// The unique identifier of the artist.
    /// </summary>
    string ArtistId { get; }

    /// <summary>
    /// The display name of the artist.
    /// </summary>
    string ArtistName { get; }

    /// <summary>
    /// The number of unique cards with unsigned copies for this artist in this set.
    /// </summary>
    int UnsignedCount { get; }

    /// <summary>
    /// The number of cards that have both signed and unsigned copies (good candidates for more signatures).
    /// </summary>
    int PartiallySignedCount { get; }

    /// <summary>
    /// The cards by this artist in this set that have unsigned copies.
    /// </summary>
    IEnumerable<ISigningCardOufEntity> Cards { get; }
}
