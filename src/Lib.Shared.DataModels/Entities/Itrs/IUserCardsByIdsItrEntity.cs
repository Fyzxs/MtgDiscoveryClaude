using System.Collections.Generic;

namespace Lib.Shared.DataModels.Entities.Itrs;

/// <summary>
/// Internal transfer representation of user cards batch query parameters.
/// This entity is used within the aggregator layer for batch point read operations,
/// representing the internal form of batch query values.
/// </summary>
public interface IUserCardsByIdsItrEntity
{
    /// <summary>
    /// The unique identifier for the user.
    /// </summary>
    string UserId { get; }

    /// <summary>
    /// The collection of card identifiers to retrieve for the user.
    /// </summary>
    ICollection<string> CardIds { get; }
}
