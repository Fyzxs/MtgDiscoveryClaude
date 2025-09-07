using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;

namespace Lib.Scryfall.Ingestion.Mappers;

internal sealed class ScryfallCardToCardItemMapper : IScryfallCardToCardItemMapper
{
    public ScryfallCardItem Map(dynamic scryfallCard) => new() { Data = scryfallCard };
}
