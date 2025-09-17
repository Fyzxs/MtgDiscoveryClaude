using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;

namespace Lib.Scryfall.Ingestion.Mappers;

internal interface ISetCardItemDynamicToExtMapper
{
    ScryfallSetCardItemExtEntity Map(dynamic scryfallCard);
}
