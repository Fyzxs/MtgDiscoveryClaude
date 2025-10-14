using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;

namespace Lib.Scryfall.Ingestion.Mappers;

internal sealed class CardItemDynamicToExtMapper : ICardItemDynamicToExtMapper
{
    public Task<ScryfallCardItemExtEntity> Map(dynamic scryfallCard)
    {
        ScryfallCardItemExtEntity result = new() { Data = scryfallCard };
        return Task.FromResult(result);
    }
}
