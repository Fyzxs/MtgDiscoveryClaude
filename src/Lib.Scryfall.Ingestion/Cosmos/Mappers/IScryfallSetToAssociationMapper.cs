using Lib.Scryfall.Ingestion.Cosmos.Entities;
using Lib.Scryfall.Shared.Apis.Models;

namespace Lib.Scryfall.Ingestion.Cosmos.Mappers;

internal interface IScryfallSetToAssociationMapper
{
    ScryfallSetAssociation Map(IScryfallSet scryfallSet);
    bool HasParentSet(IScryfallSet scryfallSet);
    bool HasNoParentSet(IScryfallSet scryfallSet);
}
