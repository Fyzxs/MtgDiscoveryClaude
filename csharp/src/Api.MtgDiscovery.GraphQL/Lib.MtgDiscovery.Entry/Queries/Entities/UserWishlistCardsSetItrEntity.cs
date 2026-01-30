using Lib.Shared.DataModels.Entities.Itrs.UserWishlistCards;

namespace Lib.MtgDiscovery.Entry.Queries.Entities;

internal sealed class UserWishlistCardsSetItrEntity : IUserWishlistCardsSetItrEntity
{
    public string UserId { get; init; }
    public string SetId { get; init; }
}
