namespace Lib.Adapter.UserWishlistCards.Apis.Entities;

public interface IRemoveUserWishlistCardXfrEntity
{
    string UserId { get; }
    string CardId { get; }
    IUserWishlistCardDetailsXfrEntity Details { get; }
}
