using Lib.Shared.DataModels.Abstractions;

namespace Lib.Adapter.UserWishlistCards.Apis.Entities;

public interface IRemoveUserWishlistCardXfrEntity : IXfrEntity
{
    string UserId { get; }
    string CardId { get; }
    IUserWishlistCardDetailsXfrEntity Details { get; }
}
