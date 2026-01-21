using System.Collections.Generic;

namespace Lib.Shared.DataModels.Entities.Itrs.UserCards;

/// <summary>
/// Represents the parameters for querying user cards by multiple artists for convention signing planning.
/// </summary>
public interface IUserCardsForSigningItrEntity
{
    /// <summary>
    /// The unique identifier of the user.
    /// </summary>
    string UserId { get; }

    /// <summary>
    /// The unique identifiers for the artists to query cards for.
    /// </summary>
    IEnumerable<string> ArtistIds { get; }
}
