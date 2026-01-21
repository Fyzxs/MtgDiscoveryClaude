using Lib.Shared.DataModels.Entities.Args.UserSealedProducts;

namespace App.MtgDiscovery.GraphQL.Entities.Args.UserSealedProducts;

public sealed class UserSealedProductDetailsArgEntity : IUserSealedProductDetailsArgEntity
{
    public int Count { get; init; }
}
