namespace Lib.Shared.DataModels.Entities.Args.UserWishlistCards;

/// <summary>
/// Argument entity for wishlist card details (finish, special, count).
/// </summary>
public interface IUserWishlistCardDetailsArgEntity
{
    string Finish { get; }
    string Special { get; }
    int Count { get; }
}
