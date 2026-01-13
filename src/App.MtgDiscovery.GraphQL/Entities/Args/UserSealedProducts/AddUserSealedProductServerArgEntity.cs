using Lib.Shared.DataModels.Entities.Args.UserSealedProducts;

namespace App.MtgDiscovery.GraphQL.Entities.Args.UserSealedProducts;

internal sealed class AddUserSealedProductServerArgEntity : IAddUserSealedProductArgEntity
{
    public string UserId { get; init; } = string.Empty;
    public string CollectionId { get; init; } = string.Empty;
    public string ProductUuid { get; init; } = string.Empty;
    public string SetId { get; init; } = string.Empty;
    public int CountDelta { get; init; }
}
