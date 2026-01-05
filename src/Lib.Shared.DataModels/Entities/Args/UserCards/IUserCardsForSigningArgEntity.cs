using System.Collections.Generic;

namespace Lib.Shared.DataModels.Entities.Args.UserCards;

/// <summary>
/// Argument entity for querying user cards by multiple artists for convention signing planning.
/// </summary>
public interface IUserCardsForSigningArgEntity
{
    /// <summary>
    /// The ID of the user whose cards to query.
    /// </summary>
    string UserId { get; }

    /// <summary>
    /// The collection of artist IDs to query cards for.
    /// </summary>
    ICollection<string> ArtistIds { get; }
}
