using Lib.Scryfall.Ingestion.Cosmos.Entities;

namespace Lib.Scryfall.Ingestion.Cosmos.Mappers;

internal interface IScryfallCardToSetCardMapper
{
    ScryfallSetCard Map(dynamic scryfallCard);
}
