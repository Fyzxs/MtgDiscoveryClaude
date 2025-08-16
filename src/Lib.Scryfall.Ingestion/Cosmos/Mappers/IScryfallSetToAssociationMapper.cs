using Lib.Scryfall.Ingestion.Apis.Models;
using Lib.Scryfall.Ingestion.Cosmos.Entities;

namespace Lib.Scryfall.Ingestion.Cosmos.Mappers;

public interface IScryfallSetToAssociationMapper
{
    ScryfallSetAssociation Map(IScryfallSet scryfallSet);
    bool HasParentSet(IScryfallSet scryfallSet);
}
