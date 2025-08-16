using Lib.Scryfall.Ingestion.Cosmos.Entities;
using Lib.Scryfall.Shared.Apis.Models;

namespace Lib.Scryfall.Ingestion.Cosmos.Mappers;

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
