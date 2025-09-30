using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;

namespace Lib.Scryfall.Ingestion.Mappers;

internal sealed class CardItemDynamicToExtMapper : ICardItemDynamicToExtMapper
{
    public ScryfallCardItemExtEntity Map(dynamic scryfallCard) => new() { Data = scryfallCard };
}
