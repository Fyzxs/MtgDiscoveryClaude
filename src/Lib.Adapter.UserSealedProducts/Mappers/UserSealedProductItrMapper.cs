using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Shared.DataModels.Entities.Itrs.UserSealedProducts;

namespace Lib.Adapter.UserSealedProducts.Mappers;

/// <summary>
/// Maps UserSealedProductExtEntity to IUserSealedProductItrEntity for query results.
/// </summary>
internal sealed class UserSealedProductItrMapper : IUserSealedProductItrMapper
{
    public UserSealedProductItrMapper()
    {
    }

    public Task<IUserSealedProductItrEntity> Map(UserSealedProductExtEntity extEntity)
    {
        IUserSealedProductItrEntity itrEntity = new UserSealedProductItrEntity
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

        return Task.FromResult(itrEntity);
    }
}
