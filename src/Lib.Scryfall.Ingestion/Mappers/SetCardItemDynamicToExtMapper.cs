using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;

namespace Lib.Scryfall.Ingestion.Mappers;

internal sealed class SetCardItemDynamicToExtMapper : ISetCardItemDynamicToExtMapper
{
    public ScryfallSetCardItemExtEntity Map(dynamic scryfallCard) => new() { Data = scryfallCard };
}
