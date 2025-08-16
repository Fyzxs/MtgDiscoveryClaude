using Lib.Scryfall.Ingestion.Apis.Models;
using Lib.Scryfall.Ingestion.Cosmos.Entities;

namespace Lib.Scryfall.Ingestion.Cosmos.Mappers;

internal interface IScryfallSetToCosmosMapper
{
    ScryfallSetItem Map(IScryfallSet scryfallSet);
}
