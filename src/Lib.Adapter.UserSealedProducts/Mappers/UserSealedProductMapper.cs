using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Cosmos.Apis.Ids;
using Lib.Cosmos.Apis.Operators;
using Lib.Shared.DataModels.Entities.Itrs.UserSealedProducts;

namespace Lib.Adapter.UserSealedProducts.Mappers;

/// <summary>
/// Maps between UserSealedProductExtEntity and IUserSealedProductItrEntity.
/// </summary>
internal sealed class UserSealedProductMapper : IUserSealedProductMapper
{
    public IUserSealedProductItrEntity Map(UserSealedProductExtEntity extEntity)
    {
        return new UserSealedProductItrEntity
        {
            UserId = extEntity.UserId,
            ProductUuid = extEntity.ProductUuid,
            ProductName = extEntity.ProductName,
            SetCode = extEntity.SetCode,
            Category = extEntity.Category,
            ImageUrl = extEntity.ImageUrl,
            Count = extEntity.Count,
            UpdatedAt = extEntity.UpdatedAt
        };
    }

    public ReadPointItem MapToReadPoint(string productUuid, string setId)
    {
        return new ReadPointItem
        {
            Id = new ProvidedCosmosItemId(productUuid),
            Partition = new ProvidedPartitionKeyValue(setId)
        };
    }
}

internal sealed class UserSealedProductItrEntity : IUserSealedProductItrEntity
{
    public string UserId { get; init; }
    public string ProductUuid { get; init; }
    public string ProductName { get; init; }
    public string SetCode { get; init; }
    public string Category { get; init; }
    public string ImageUrl { get; init; }
    public int Count { get; init; }
    public string UpdatedAt { get; init; }
}
