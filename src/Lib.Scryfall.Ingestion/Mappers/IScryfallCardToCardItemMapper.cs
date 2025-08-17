using Lib.Adapter.Scryfall.Cosmos.Apis.Entities;

namespace Lib.Scryfall.Ingestion.Mappers;

internal interface IScryfallCardToCardItemMapper
{
    ScryfallCardItem Map(dynamic scryfallCard);
}
