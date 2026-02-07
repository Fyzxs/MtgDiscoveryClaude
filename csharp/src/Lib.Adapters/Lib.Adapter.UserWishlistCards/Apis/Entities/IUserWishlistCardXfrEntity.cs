using Lib.Shared.DataModels.Abstractions;

namespace Lib.Adapter.UserWishlistCards.Apis.Entities;

public interface IUserWishlistCardXfrEntity : IXfrEntity
{
    string UserId { get; }
}
