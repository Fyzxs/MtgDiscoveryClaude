using System.Collections.Generic;

namespace Lib.Adapter.UserCards.Apis.Entities;

/// <summary>
/// Transfer representation of user cards batch query parameters used by the adapter layer.
/// This entity crosses the Aggregator→Adapter boundary for batch point read operations,
/// providing a simple wrapper for user cards batch query values in external system operations.
/// </summary>
public interface IUserCardsByIdsXfrEntity
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
