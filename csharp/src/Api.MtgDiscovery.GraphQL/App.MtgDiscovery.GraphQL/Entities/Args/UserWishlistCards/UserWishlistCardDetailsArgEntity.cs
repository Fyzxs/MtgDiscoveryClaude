using Lib.Shared.DataModels.Entities.Args.UserWishlistCards;

namespace App.MtgDiscovery.GraphQL.Entities.Args.UserWishlistCards;

internal sealed class UserWishlistCardDetailsArgEntity : IUserWishlistCardDetailsArgEntity
{
    public string Finish { get; init; }
    public string Special { get; init; }
    public int Count { get; init; }
}
