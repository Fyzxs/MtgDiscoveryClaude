using Lib.Adapter.UserWishlistCards.Apis.Entities;

namespace Lib.Adapter.UserWishlistCards.Tests.Fakes;

public sealed class UserWishlistCardDetailsXfrEntityFake : IUserWishlistCardDetailsXfrEntity
{
    public string Finish { get; init; }
    public string Special { get; init; }
    public int Count { get; init; }
    public string CacheKey => $"user_wishlist_card_details:{Finish}:{Special}";
}
