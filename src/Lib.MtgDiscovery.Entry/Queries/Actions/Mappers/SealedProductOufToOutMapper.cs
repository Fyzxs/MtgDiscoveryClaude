using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities.Outs.SealedProducts;
using Lib.Shared.DataModels.Entities.Oufs.SealedProducts;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Mappers;

internal sealed class SealedProductOufToOutMapper : ISealedProductOufToOutMapper
{
    public Task<SealedProductOutEntity> Map(ISealedProductOufEntity source)
    {
        SealedProductOutEntity result = new()
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
