using Lib.Shared.DataModels.Entities.Itrs.UserWishlistCards;

namespace Lib.MtgDiscovery.Entry.Commands.Entities;

internal sealed class UserWishlistCardDetailsItrEntity : IUserWishlistCardDetailsItrEntity
{
    public string Finish { get; init; }
    public string Special { get; init; }
    public int Count { get; init; }
}
