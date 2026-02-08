using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserSealedProducts;
using Lib.Aggregator.UserSealedProducts.Entities;
using Lib.Shared.DataModels.Entities.Oufs.SealedProducts;

namespace Lib.Aggregator.UserSealedProducts.Commands.Mappers;

internal sealed class UserSealedProductExtToOufMapper : IUserSealedProductExtToOufMapper
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
}
