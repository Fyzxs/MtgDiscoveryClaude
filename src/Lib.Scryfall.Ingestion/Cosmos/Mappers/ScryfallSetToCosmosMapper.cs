using Lib.Scryfall.Ingestion.Apis.Models;
using Lib.Scryfall.Ingestion.Cosmos.Entities;

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
