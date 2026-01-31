using Lib.Shared.DataModels.Entities.Oufs.UserWishlistCards;

namespace Lib.Aggregator.UserWishlistCards.Entities;

internal sealed class UserWishlistCardDetailsOufEntity : IUserWishlistCardDetailsOufEntity
{
    public string Finish { get; init; }
    public string Special { get; init; }
    public int Count { get; init; }
}
