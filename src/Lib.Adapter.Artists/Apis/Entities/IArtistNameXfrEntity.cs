using System.Collections.Generic;

namespace Lib.Adapter.Artists.Apis.Entities;

/// <summary>
/// Transfer representation of an artist's name used by the adapter layer.
/// Provides the original artist name, a normalized form suitable for comparisons,
/// and a collection of trigrams used to support trigram-based search/matching.
/// </summary>
public interface IArtistNameXfrEntity
{
    /// <summary>
    /// The artist's original display name as provided by the source.
    /// </summary>
    string ArtistName { get; }

    /// <summary>
    /// A normalized form of <see cref="ArtistName"/> (for example, lower-cased and trimmed)
    /// used for deterministic comparisons and indexing.
    /// </summary>
    string Normalized { get; }

    /// <summary>
    /// A collection of three-character tokens (trigrams) derived from the artist name.
    /// Used to support approximate or partial-name searches and matching.
    /// Implementations may return an empty collection when no trigrams are available.
    /// </summary>
    ICollection<string> Trigrams { get; }
}

