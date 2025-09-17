using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Scryfall.Shared.Apis.Models;

namespace Lib.Scryfall.Ingestion.Mappers;

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

    public ScryfallSetParentAssociationExtArg Map(IScryfallSet scryfallSet)
    {
        return new ScryfallSetParentAssociationExtArg
        {
            SetId = scryfallSet.Id(),
            ParentSetCode = scryfallSet.ParentSetCode(),
            SetCode = scryfallSet.Code(),
            SetName = scryfallSet.Name()
        };
    }
}
