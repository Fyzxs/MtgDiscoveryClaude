using Lib.Adapter.Scryfall.Cosmos.Apis.Entities;
using Lib.Scryfall.Shared.Apis.Models;

namespace Lib.Scryfall.Ingestion.Mappers;

internal interface IScryfallSetToAssociationMapper
{
    ScryfallSetParentAssociationItem Map(IScryfallSet scryfallSet);
    bool HasParentSet(IScryfallSet scryfallSet);
    bool HasNoParentSet(IScryfallSet scryfallSet);
}
