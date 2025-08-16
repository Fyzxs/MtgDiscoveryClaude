using Lib.Scryfall.Ingestion.Cosmos.Entities;

namespace Lib.Scryfall.Ingestion.Cosmos.Mappers;

internal interface IScryfallCardToCardItemMapper
{
    ScryfallCardItem Map(dynamic scryfallCard);
}