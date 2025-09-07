using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;

namespace Lib.Scryfall.Ingestion.Mappers;

internal interface IScryfallCardToSetCardMapper
{
    ScryfallSetCardItem Map(dynamic scryfallCard);
}
