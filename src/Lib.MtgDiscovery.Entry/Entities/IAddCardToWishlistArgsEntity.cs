using Lib.Shared.DataModels.Entities.Args.User;
using Lib.Shared.DataModels.Entities.Args.UserWishlistCards;

namespace Lib.MtgDiscovery.Entry.Entities;

public interface IAddCardToWishlistArgsEntity
{
    IAuthUserArgEntity AuthUser { get; }
    IAddUserWishlistCardArgEntity AddUserWishlistCard { get; }
}
