using Lib.Adapter.Scryfall.Cosmos.Apis.Entities;
using Lib.Scryfall.Shared.Apis.Models;

namespace Lib.Scryfall.Ingestion.Mappers;

internal interface IScryfallSetToCosmosMapper
{
    ScryfallSetItem Map(IScryfallSet scryfallSet);
}
