using Lib.Scryfall.Ingestion.Apis.Models;
using Lib.Scryfall.Ingestion.Cosmos.Entities;

namespace Lib.Scryfall.Ingestion.Cosmos.Mappers;

internal sealed class ScryfallSetToAssociationMapper : IScryfallSetToAssociationMapper
{
    public bool HasParentSet(IScryfallSet scryfallSet)
    {
        dynamic data = scryfallSet.Data();
        string parentSetCode = data.parent_set_code ?? null;
        return parentSetCode is not null;
    }

    public ScryfallSetAssociation Map(IScryfallSet scryfallSet)
    {
        dynamic data = scryfallSet.Data();

        return new ScryfallSetAssociation
        {
            SetId = data.id,
            ParentSetCode = data.parent_set_code,
            SetCode = data.code,
            SetName = data.name
        };
    }
}
