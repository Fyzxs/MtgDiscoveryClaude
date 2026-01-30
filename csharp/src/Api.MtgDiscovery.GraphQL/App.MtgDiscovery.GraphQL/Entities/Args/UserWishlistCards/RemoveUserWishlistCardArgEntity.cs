using Lib.Shared.DataModels.Entities.Args.UserWishlistCards;

namespace App.MtgDiscovery.GraphQL.Entities.Args.UserWishlistCards;

public sealed class RemoveUserWishlistCardArgEntity : IRemoveUserWishlistCardArgEntity
{
    public string CardId { get; init; }
    public string UserId { get; init; }

    public UserWishlistCardDetailsArgEntity UserWishlistCardDetails { get; init; }

    IUserWishlistCardDetailsArgEntity IRemoveUserWishlistCardArgEntity.UserWishlistCardDetails => UserWishlistCardDetails;
}
