using System.Collections.Generic;

namespace Lib.Adapter.Artists.Apis.Entities;

/// <summary>
/// Transfer representation of artist search terms used by the adapter layer for trigram-based searches.
/// This entity crosses the Aggregator→Adapter boundary containing preprocessed search data
/// optimized for external system queries.
/// </summary>
public interface IArtistSearchTermXrfEntity
{
    /// <summary>
    /// Collection of three-character tokens (trigrams) derived from the search term.
    /// Used for trigram-based matching against artist name indexes in the external system.
    /// </summary>
    ICollection<string> SearchTerms { get; }

    /// <summary>
    /// Normalized form of the original search term (typically lowercase, letters only).
    /// Used for server-side filtering and exact matching within trigram results.
    /// </summary>
    string Normalized { get; }
}
