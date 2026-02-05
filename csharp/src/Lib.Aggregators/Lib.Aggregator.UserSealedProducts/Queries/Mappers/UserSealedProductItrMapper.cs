using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserSealedProducts;
using Lib.Shared.DataModels.Entities.Itrs.UserSealedProducts;

namespace Lib.Aggregator.UserSealedProducts.Queries.Mappers;

internal sealed class UserSealedProductItrMapper : IUserSealedProductItrMapper
{
    public Task<IUserSealedProductItrEntity> Map(UserSealedProductExtEntity input)
    {
        IUserSealedProductItrEntity itr = new UserSealedProductItrEntity
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
        return Task.FromResult(itr);
    }

    private sealed class UserSealedProductItrEntity : IUserSealedProductItrEntity
    {
        public required string UserId { get; init; }
        public required string ProductUuid { get; init; }
        public required string ProductName { get; init; }
        public required string SetCode { get; init; }
        public required string Category { get; init; }
        public required string ImageUrl { get; init; }
        public required int Count { get; init; }
        public required string UpdatedAt { get; init; }
    }
}
