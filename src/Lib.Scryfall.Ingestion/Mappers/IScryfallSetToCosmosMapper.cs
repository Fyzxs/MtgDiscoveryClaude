using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Scryfall.Shared.Apis.Models;

namespace Lib.Scryfall.Ingestion.Mappers;

internal interface IScryfallSetToCosmosMapper
{
    ScryfallSetExtArg Map(IScryfallSet scryfallSet);
}
