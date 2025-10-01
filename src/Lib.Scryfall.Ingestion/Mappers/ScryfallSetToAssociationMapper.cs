using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Scryfall.Shared.Apis.Models;

namespace Lib.Scryfall.Ingestion.Mappers;

internal sealed class SetToSetParentAssociationExtMapper : ISetToSetParentAssociationExtMapper
{
    public bool HasParentSet(IScryfallSet scryfallSet) => scryfallSet.HasParentSet();

    public bool HasNoParentSet(IScryfallSet scryfallSet) => HasParentSet(scryfallSet) is false;

    public ScryfallSetParentAssociationExtEntity Map(IScryfallSet scryfallSet)
    {
        return new ScryfallSetParentAssociationExtEntity
        {
            SetId = scryfallSet.Id(),
            ParentSetCode = scryfallSet.ParentSetCode(),
            SetCode = scryfallSet.Code(),
            SetName = scryfallSet.Name()
        };
    }
}
