using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Scryfall.Shared.Apis.Models;

namespace Lib.Scryfall.Ingestion.Mappers;

// NOTE: Cannot extend ICreateMapper<IScryfallSet, ScryfallSetParentAssociationExtEntity> because
// this interface provides additional validation methods (HasParentSet, HasNoParentSet) beyond mapping.
// This is a legitimate exception to the standard mapper pattern.
internal interface ISetToSetParentAssociationExtMapper
{
    ScryfallSetParentAssociationExtEntity Map(IScryfallSet scryfallSet);
    bool HasParentSet(IScryfallSet scryfallSet);
    bool HasNoParentSet(IScryfallSet scryfallSet);
}
