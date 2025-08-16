using Lib.Scryfall.Ingestion.Cosmos.Entities;
using Lib.Scryfall.Shared.Apis.Models;

namespace Lib.Scryfall.Ingestion.Cosmos.Mappers;

internal interface IScryfallSetToCosmosMapper
{
    ScryfallSetItem Map(IScryfallSet scryfallSet);
}
