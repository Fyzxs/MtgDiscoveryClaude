using Lib.Shared.DataModels.Entities.Args.User;
using Lib.Shared.DataModels.Entities.Args.UserWishlistCards;

namespace Lib.MtgDiscovery.Entry.Entities;

public sealed class AddCardToWishlistArgsEntity : IAddCardToWishlistArgsEntity
{
    public IAuthUserArgEntity AuthUser { get; init; }
    public IAddUserWishlistCardArgEntity AddUserWishlistCard { get; init; }
}
