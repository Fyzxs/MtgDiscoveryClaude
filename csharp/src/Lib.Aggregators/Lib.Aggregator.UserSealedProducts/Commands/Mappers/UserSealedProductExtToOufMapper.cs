using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserSealedProducts;
using Lib.Shared.DataModels.Entities.Oufs.SealedProducts;

namespace Lib.Aggregator.UserSealedProducts.Commands.Mappers;

internal sealed class UserSealedProductExtToSealedProductOufMapper : IUserSealedProductExtToSealedProductOufMapper
{
    public Task<ISealedProductOufEntity> Map(UserSealedProductExtEntity input)
    {
        ISealedProductOufEntity ouf = new SealedProductOufEntity
        {
            Uuid = input.ProductUuid,
            SetId = input.SetId,
            SetCode = input.SetCode,
            SetName = input.SetName,
            Name = input.ProductName,
            Category = input.Category,
            Subtype = input.Subtype,
            CardCount = input.CardCount,
            ReleaseDate = input.ReleaseDate,
            TcgplayerProductId = input.TcgplayerProductId,
            ImageUrl = input.ImageUrl,
            PurchaseUrlTcgplayer = input.PurchaseUrlTcgplayer,
            PurchaseUrlCardmarket = input.PurchaseUrlCardmarket,
            PurchaseUrlCardKingdom = input.PurchaseUrlCardKingdom,
            UserQuantity = input.Count
        };
        return Task.FromResult(ouf);
    }

    private sealed class SealedProductOufEntity : ISealedProductOufEntity
    {
        public required string Uuid { get; init; }
        public required string SetId { get; init; }
        public required string SetCode { get; init; }
        public required string SetName { get; init; }
        public required string Name { get; init; }
        public required string Category { get; init; }
        public required string Subtype { get; init; }
        public required int? CardCount { get; init; }
        public required string ReleaseDate { get; init; }
        public required string TcgplayerProductId { get; init; }
        public required string ImageUrl { get; init; }
        public required string PurchaseUrlTcgplayer { get; init; }
        public required string PurchaseUrlCardmarket { get; init; }
        public required string PurchaseUrlCardKingdom { get; init; }
        public required int UserQuantity { get; init; }
    }
}
