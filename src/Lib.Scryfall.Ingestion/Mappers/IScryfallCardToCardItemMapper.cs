using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;

namespace Lib.Scryfall.Ingestion.Mappers;

internal interface IScryfallCardToCardItemMapper
{
    ScryfallCardItem Map(dynamic scryfallCard);
}
