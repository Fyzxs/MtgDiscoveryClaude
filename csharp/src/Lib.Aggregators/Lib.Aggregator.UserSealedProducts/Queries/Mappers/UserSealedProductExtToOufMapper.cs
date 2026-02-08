using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserSealedProducts;
using Lib.Shared.DataModels.Entities.Oufs.UserSealedProducts;

namespace Lib.Aggregator.UserSealedProducts.Queries.Mappers;

internal sealed class UserSealedProductExtToOufMapper : IUserSealedProductExtToOufMapper
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

    private sealed class UserSealedProductOufEntity : IUserSealedProductOufEntity
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
}
