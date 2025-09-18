using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;

namespace Lib.Scryfall.Ingestion.Mappers;

internal interface ICardItemDynamicToExtMapper
{
    ScryfallCardItemExtEntity Map(dynamic scryfallCard);
}
