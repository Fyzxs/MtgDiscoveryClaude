using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;

namespace Lib.Scryfall.Ingestion.Mappers;

internal interface IScryfallCardToCardItemMapper
{
    ScryfallCardExtArg Map(dynamic scryfallCard);
}
