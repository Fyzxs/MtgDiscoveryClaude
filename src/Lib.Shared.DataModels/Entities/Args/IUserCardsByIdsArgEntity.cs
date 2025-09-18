using System.Collections.Generic;

namespace Lib.Shared.DataModels.Entities.Args;

/// <summary>
/// Argument entity for querying multiple user cards by their IDs.
/// </summary>
public interface IUserCardsByIdsArgEntity
{
    /// <summary>
    /// The ID of the user whose cards to query.
    /// </summary>
    string UserId { get; }

    /// <summary>
    /// The collection of card IDs to query.
    /// </summary>
    ICollection<string> CardIds { get; }
}
