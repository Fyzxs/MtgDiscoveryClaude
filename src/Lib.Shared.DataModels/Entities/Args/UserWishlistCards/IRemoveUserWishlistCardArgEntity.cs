namespace Lib.Shared.DataModels.Entities.Args.UserWishlistCards;

/// <summary>
/// Argument entity for removing a card from a user's wishlist.
/// </summary>
public interface IRemoveUserWishlistCardArgEntity
{
    string CardId { get; }
    string UserId { get; }
    IUserWishlistCardDetailsArgEntity UserWishlistCardDetails { get; }
}
