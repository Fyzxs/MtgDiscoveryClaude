using Lib.Shared.DataModels.Entities.Args.SealedProducts;

namespace App.MtgDiscovery.GraphQL.Entities.Args.SealedProducts;

public sealed class GetSealedProductsBySetCodeArgEntity : ISealedProductsBySetCodeArgEntity
{
    public string SetCode { get; init; } = string.Empty;
    public string UserId { get; init; } = string.Empty;
    public string CollectionId { get; init; } = string.Empty;

    public bool HasCollectionId => string.IsNullOrEmpty(CollectionId) is false;
}
