using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Shared.DataModels.Entities.Oufs.UserSealedProducts;

namespace Lib.Aggregator.UserSealedProducts.Commands.Mappers;

internal sealed class UserSealedProductOufMapper : IUserSealedProductOufMapper
{
    public Task<IUserSealedProductOufEntity> Map(UserSealedProductExtEntity input)
    {
        IUserSealedProductOufEntity ouf = new UserSealedProductOufEntity
        {
            ProductUuid = input.ProductUuid,
            Count = input.Count
        };
        return Task.FromResult(ouf);
    }

    private sealed class UserSealedProductOufEntity : IUserSealedProductOufEntity
    {
        public required string ProductUuid { get; init; }
        public required int Count { get; init; }
    }
}
