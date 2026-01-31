using Lib.Shared.DataModels.Entities.Args.UserWishlistCards;

namespace App.MtgDiscovery.GraphQL.Entities.Args.UserWishlistCards;

internal sealed class AddUserWishlistCardArgEntity : IAddUserWishlistCardArgEntity
{
    public string CardId { get; init; }
    public string SetId { get; init; }
    public string UserId { get; init; }

    public UserWishlistCardDetailsArgEntity UserWishlistCardDetails { get; init; }

    IUserWishlistCardDetailsArgEntity IAddUserWishlistCardArgEntity.UserWishlistCardDetails => UserWishlistCardDetails;
}
