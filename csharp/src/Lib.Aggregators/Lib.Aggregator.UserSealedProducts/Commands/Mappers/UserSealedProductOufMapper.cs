using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserSealedProducts;
using Lib.Shared.DataModels.Entities.Oufs.UserSealedProducts;

namespace Lib.Aggregator.UserSealedProducts.Commands.Mappers;

internal sealed class UserSealedProductOufMapper : IUserSealedProductOufMapper
{
    public Task<IUserSealedProductOufEntity> Map(UserSealedProductExtEntity input)
    {
        IUserSealedProductOufEntity ouf = new UserSealedProductOufEntity
        {
            UserId = input.UserId,
            ProductUuid = input.ProductUuid,
            ProductName = input.ProductName,
            SetCode = input.SetCode,
            Category = input.Category,
            ImageUrl = input.ImageUrl,
            Count = input.Count,
            UpdatedAt = input.UpdatedAt
        };
        return Task.FromResult(ouf);
    }
}
