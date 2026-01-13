using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Shared.DataModels.Entities.Oufs.UserSealedProducts;

namespace Lib.Adapter.UserSealedProducts.Mappers;

/// <summary>
/// Maps UserSealedProductExtEntity to IUserSealedProductOufEntity for response output.
/// </summary>
internal sealed class UserSealedProductOufMapper : IUserSealedProductOufMapper
{
    public UserSealedProductOufMapper()
    {
    }

    public Task<IUserSealedProductOufEntity> Map(UserSealedProductExtEntity extEntity)
    {
        IUserSealedProductOufEntity oufEntity = new UserSealedProductOufEntity
        {
            ProductUuid = extEntity.ProductUuid,
            Count = extEntity.Count
        };

        return Task.FromResult(oufEntity);
    }
}
