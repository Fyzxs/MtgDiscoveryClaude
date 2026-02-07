using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.SealedProducts;
using Lib.Aggregator.SealedProducts.Queries.Entities;
using Lib.Shared.DataModels.Entities.Oufs.SealedProducts;

namespace Lib.Aggregator.SealedProducts.Queries.Mappers;

internal sealed class SealedProductExtToOufMapper : ISealedProductExtToOufMapper
{
    public Task<ISealedProductOufEntity> Map([NotNull] SealedProductExtEntity source)
    {
        ISealedProductOufEntity result = new SealedProductOufEntity
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

        return Task.FromResult(result);
    }
}
