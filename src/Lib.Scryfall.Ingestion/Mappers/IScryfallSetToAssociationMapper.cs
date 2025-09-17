using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Scryfall.Shared.Apis.Models;

namespace Lib.Scryfall.Ingestion.Mappers;

internal interface IScryfallSetToAssociationMapper
{
    ScryfallSetParentAssociationExtArg Map(IScryfallSet scryfallSet);
    bool HasParentSet(IScryfallSet scryfallSet);
    bool HasNoParentSet(IScryfallSet scryfallSet);
}
