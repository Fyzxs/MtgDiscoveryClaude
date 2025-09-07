using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;

namespace Lib.Scryfall.Ingestion.Mappers;

internal sealed class ScryfallCardToSetCardMapper : IScryfallCardToSetCardMapper
{
    public ScryfallSetCardItem Map(dynamic scryfallCard) => new() { Data = scryfallCard };
}
