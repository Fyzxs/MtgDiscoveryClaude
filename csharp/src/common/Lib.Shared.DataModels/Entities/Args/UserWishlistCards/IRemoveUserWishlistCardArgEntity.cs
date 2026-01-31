using Lib.Shared.DataModels.Entities.Models;

namespace Lib.Shared.DataModels.Entities.Args.UserWishlistCards;

public interface IRemoveUserWishlistCardArgEntity : IUserIdArgModel
{
    string CardId { get; }
    IUserWishlistCardDetailsArgEntity UserWishlistCardDetails { get; }
}
