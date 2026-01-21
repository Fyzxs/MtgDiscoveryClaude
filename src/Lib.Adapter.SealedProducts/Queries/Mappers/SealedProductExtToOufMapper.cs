using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.SealedProducts.Apis.Entities;
using Lib.Shared.DataModels.Entities.Oufs.SealedProducts;

namespace Lib.Adapter.SealedProducts.Queries.Mappers;

internal sealed class SealedProductExtToOufMapper : ISealedProductExtToOufMapper
{
    public ISealedProductOufEntity Map(SealedProductExtEntity source) =>
        new SealedProductOufEntity
        {
            Uuid = source.Uuid,
            SetId = source.SetId,
            SetCode = source.SetCode,
            SetName = source.SetName,
            Name = source.Name,
            Category = source.Category,
            Subtype = source.Subtype,
            CardCount = source.CardCount,
            ReleaseDate = source.ReleaseDate,
            TcgplayerProductId = source.TcgplayerProductId,
            ImageUrl = source.ImageUrl,
            PurchaseUrlTcgplayer = source.PurchaseUrlTcgplayer,
            PurchaseUrlCardmarket = source.PurchaseUrlCardmarket,
            PurchaseUrlCardKingdom = source.PurchaseUrlCardKingdom
        };
}
