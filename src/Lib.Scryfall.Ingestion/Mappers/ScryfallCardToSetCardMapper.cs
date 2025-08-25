using Lib.Adapter.Scryfall.Cosmos.Apis.Entities;

namespace Lib.Scryfall.Ingestion.Mappers;

internal sealed class ScryfallCardToSetCardMapper : IScryfallCardToSetCardMapper
{
    public ScryfallSetCard Map(dynamic scryfallCard) => new() { Scryfall = scryfallCard };
}
