using System.Collections.Generic;

namespace Lib.Adapter.UserCards.Apis.Entities;

/// <summary>
/// Transfer representation of user cards for signing query parameters used by the adapter layer.
/// This entity crosses the Aggregator→Adapter boundary when querying cards by multiple artists
/// for convention signing planning functionality.
/// </summary>
public interface IUserCardsForSigningXfrEntity
{
    /// <summary>
    /// The unique identifier for the user.
    /// </summary>
    string UserId { get; }

    /// <summary>
    /// The unique identifiers for the artists to query cards for.
    /// </summary>
    IEnumerable<string> ArtistIds { get; }
}
