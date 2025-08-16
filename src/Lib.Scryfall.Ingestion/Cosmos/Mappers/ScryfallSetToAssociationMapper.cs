using Lib.Scryfall.Ingestion.Apis.Models;
using Lib.Scryfall.Ingestion.Cosmos.Entities;

namespace Lib.Scryfall.Ingestion.Cosmos.Mappers;

internal sealed class ScryfallSetToAssociationMapper : IScryfallSetToAssociationMapper
{
    public bool HasParentSet(IScryfallSet scryfallSet)
    {
        return scryfallSet.HasParentSet();
    }

    public bool HasNoParentSet(IScryfallSet scryfallSet)
    {
        return HasParentSet(scryfallSet) is false;
    }

    public ScryfallSetAssociation Map(IScryfallSet scryfallSet)
    {
        return new ScryfallSetAssociation
        {
            SetId = scryfallSet.Id(),
            ParentSetCode = scryfallSet.ParentSetCode(),
            SetCode = scryfallSet.Code(),
            SetName = scryfallSet.Name()
        };
    }
}
