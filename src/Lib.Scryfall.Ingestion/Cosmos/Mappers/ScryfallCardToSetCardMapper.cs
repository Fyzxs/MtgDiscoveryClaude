using Lib.Scryfall.Ingestion.Cosmos.Entities;

namespace Lib.Scryfall.Ingestion.Cosmos.Mappers;

internal sealed class ScryfallCardToSetCardMapper : IScryfallCardToSetCardMapper
{
    public ScryfallSetCard Map(dynamic scryfallCard) => new() { Data = scryfallCard };
}
