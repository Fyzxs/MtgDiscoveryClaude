namespace Lib.Shared.DataModels.Entities.Args.UserWishlistCards;

/// <summary>
/// Argument entity for adding a card to a user's wishlist.
/// </summary>
public interface IAddUserWishlistCardArgEntity
{
    string CardId { get; }
    string SetId { get; }
    string UserId { get; }
    IUserWishlistCardDetailsArgEntity UserWishlistCardDetails { get; }
}
