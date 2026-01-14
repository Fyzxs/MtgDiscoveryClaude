using Lib.Shared.DataModels.Entities.Args.UserSealedProducts;

namespace App.MtgDiscovery.GraphQL.Entities.Args.UserSealedProducts;

public sealed class AddUserSealedProductArgEntity : IAddUserSealedProductArgEntity
{
    public string ProductUuid { get; init; }
    public string SetId { get; init; }
    public string UserId { get; init; }

    // GraphQL needs concrete type for input, not interface
    public UserSealedProductDetailsArgEntity UserSealedProductDetails { get; init; }

    // Implement interface property
    IUserSealedProductDetailsArgEntity IAddUserSealedProductArgEntity.UserSealedProductDetails => UserSealedProductDetails;
}
