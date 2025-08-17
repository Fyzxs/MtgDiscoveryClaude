using Lib.Adapter.Scryfall.Cosmos.Apis.Entities;

namespace Lib.Scryfall.Ingestion.Mappers;

internal interface IScryfallCardToSetCardMapper
{
    ScryfallSetCard Map(dynamic scryfallCard);
}
