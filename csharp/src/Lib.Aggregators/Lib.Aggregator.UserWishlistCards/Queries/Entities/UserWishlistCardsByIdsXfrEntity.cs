using System.Collections.Generic;
using Lib.Adapter.UserWishlistCards.Apis.Entities;

namespace Lib.Aggregator.UserWishlistCards.Queries.Entities;

internal sealed class UserWishlistCardsByIdsXfrEntity : IUserWishlistCardsByIdsXfrEntity
{
    public string UserId { get; init; }
    public ICollection<string> CardIds { get; init; }
    public string CacheKey => $"user_wishlist_cards:ids:{UserId}";
}
