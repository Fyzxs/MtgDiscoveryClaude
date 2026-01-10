namespace Lib.Shared.DataModels.Entities.Itrs.UserWishlistCards;

/// <summary>
/// Internal transfer representation of user wishlist cards query parameters.
/// This entity is used within the domain and aggregator layers for querying,
/// representing the internal form of query values.
/// </summary>
public interface IUserWishlistCardsQueryItrEntity
{
    /// <summary>
    /// The unique identifier for the user whose wishlist to query.
    /// </summary>
    string UserId { get; }
}
