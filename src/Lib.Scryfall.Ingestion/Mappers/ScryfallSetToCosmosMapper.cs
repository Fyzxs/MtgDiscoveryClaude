using Lib.Adapter.Scryfall.Cosmos.Apis.Entities;
using Lib.Scryfall.Shared.Apis.Models;

namespace Lib.Scryfall.Ingestion.Mappers;

internal sealed class ScryfallSetToCosmosMapper : IScryfallSetToCosmosMapper
{
    public ScryfallSetItem Map(IScryfallSet scryfallSet)
    {
        return new ScryfallSetItem
        {
            Data = scryfallSet.Data()
        };
    }
}
