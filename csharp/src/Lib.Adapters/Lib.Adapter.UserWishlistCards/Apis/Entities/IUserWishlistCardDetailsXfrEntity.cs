using Lib.Shared.DataModels.Abstractions;

namespace Lib.Adapter.UserWishlistCards.Apis.Entities;

public interface IUserWishlistCardDetailsXfrEntity : IXfrEntity
{
    string Finish { get; }
    string Special { get; }
    int Count { get; }
}
