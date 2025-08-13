using Lib.Scryfall.Ingestion.Apis.Models;
using Lib.Scryfall.Ingestion.Cosmos.Entities;

namespace Lib.Scryfall.Ingestion.Cosmos.Mappers;

public interface IScryfallSetToCosmosMapper
{
    ScryfallSetItem Map(IScryfallSet scryfallSet);
}
