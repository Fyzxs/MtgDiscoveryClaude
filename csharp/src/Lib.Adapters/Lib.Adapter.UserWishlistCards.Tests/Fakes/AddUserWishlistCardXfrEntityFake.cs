using System.Collections.Generic;
using Lib.Adapter.UserWishlistCards.Apis.Entities;

namespace Lib.Adapter.UserWishlistCards.Tests.Fakes;

public sealed class AddUserWishlistCardXfrEntityFake : IAddUserWishlistCardXfrEntity
{
    public string UserId { get; init; }
    public string CardId { get; init; }
    public string SetId { get; init; }
    public string CardName { get; init; }
    public string SetName { get; init; }
    public string SetCode { get; init; }
    public IEnumerable<string> ArtistIds { get; init; }
    public string CardNameGuid { get; init; }
    public IUserWishlistCardDetailsXfrEntity Details { get; init; }
    public string CacheKey => $"add_user_wishlist_card:{UserId}:{CardId}";
}
