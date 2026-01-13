using Lib.Shared.DataModels.Entities.Args.UserSealedProducts;

namespace App.MtgDiscovery.GraphQL.Entities.Args.UserSealedProducts;

public sealed class AddUserSealedProductArgEntity : IAddUserSealedProductClientArgEntity
{
    public string ProductUuid { get; init; } = string.Empty;
    public string SetId { get; init; } = string.Empty;
    public int CountDelta { get; init; }
}
