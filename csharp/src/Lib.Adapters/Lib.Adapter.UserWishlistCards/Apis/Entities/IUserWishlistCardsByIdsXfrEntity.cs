using System.Collections.Generic;
using Lib.Shared.DataModels.Abstractions;

namespace Lib.Adapter.UserWishlistCards.Apis.Entities;

/// <summary>
/// Transfer representation of user wishlist cards batch query parameters used by the adapter layer.
/// This entity crosses the Aggregator→Adapter boundary for batch point read operations,
/// providing a simple wrapper for user wishlist cards batch query values in external system operations.
/// </summary>
public interface IUserWishlistCardsByIdsXfrEntity : IXfrEntity
{
    /// <summary>
    /// The unique identifier for the user.
    /// </summary>
    string UserId { get; }

    /// <summary>
    /// The collection of card identifiers to retrieve from the user's wishlist.
    /// </summary>
    ICollection<string> CardIds { get; }
}
