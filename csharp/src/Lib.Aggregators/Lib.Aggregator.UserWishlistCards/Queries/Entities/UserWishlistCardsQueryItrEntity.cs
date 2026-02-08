using Lib.Shared.DataModels.Entities.Itrs.UserWishlistCards;

namespace Lib.Aggregator.UserWishlistCards.Queries.Entities;

internal sealed class UserWishlistCardsQueryItrEntity : IUserWishlistCardsQueryItrEntity
{
    public string UserId { get; init; }
}
