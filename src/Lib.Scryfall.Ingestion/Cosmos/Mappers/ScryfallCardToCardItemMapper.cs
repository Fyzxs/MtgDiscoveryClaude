using Lib.Scryfall.Ingestion.Cosmos.Entities;

namespace Lib.Scryfall.Ingestion.Cosmos.Mappers;

internal sealed class ScryfallCardToCardItemMapper : IScryfallCardToCardItemMapper
{
    public ScryfallCardItem Map(dynamic scryfallCard) => new() { Data = scryfallCard };
}