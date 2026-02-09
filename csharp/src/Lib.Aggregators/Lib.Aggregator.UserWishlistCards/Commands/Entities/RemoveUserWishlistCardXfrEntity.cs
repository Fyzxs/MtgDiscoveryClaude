using Lib.Adapter.UserWishlistCards.Apis.Entities;

namespace Lib.Aggregator.UserWishlistCards.Commands.Entities;

internal sealed class RemoveUserWishlistCardXfrEntity : IRemoveUserWishlistCardXfrEntity
{
    public string UserId { get; init; }
    public string CardId { get; init; }
    public IUserWishlistCardDetailsXfrEntity Details { get; init; }
    public string CacheKey => $"remove_user_wishlist_card:{UserId}:{CardId}";
}
