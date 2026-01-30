using Lib.Shared.DataModels.Entities.Models;

namespace Lib.Shared.DataModels.Entities.Args.UserWishlistCards;

public interface IAddUserWishlistCardArgEntity : IUserIdArgModel
{
    string CardId { get; }
    string SetId { get; }
    IUserWishlistCardDetailsArgEntity UserWishlistCardDetails { get; }
}
