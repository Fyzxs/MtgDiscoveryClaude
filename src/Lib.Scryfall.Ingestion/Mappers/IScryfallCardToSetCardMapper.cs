using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;

namespace Lib.Scryfall.Ingestion.Mappers;

internal interface IScryfallCardToSetCardMapper
{
    ScryfallSetCardItemExtArg Map(dynamic scryfallCard);
}
