using Lib.Adapter.UserWishlistCards.Apis.Entities;

namespace Lib.Aggregator.UserWishlistCards.Queries.Entities;

internal sealed class UserWishlistCardsQueryXfrEntity : IUserWishlistCardXfrEntity
{
    public string UserId { get; init; }
    public string CacheKey => $"user_wishlist_card:{UserId}";
}
