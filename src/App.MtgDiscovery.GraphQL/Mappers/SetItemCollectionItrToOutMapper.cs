using System.Collections.Generic;
using System.Threading.Tasks;
using App.MtgDiscovery.GraphQL.Entities.Outs.Sets;
using Lib.Shared.DataModels.Entities;

namespace App.MtgDiscovery.GraphQL.Mappers;

internal sealed class SetItemCollectionItrToOutMapper : ISetItemCollectionItrToOutMapper
{
    private readonly ISetItemItrToOutMapper _mapper;

    public SetItemCollectionItrToOutMapper() : this(new SetItemItrToOutMapper())
    { }

    private SetItemCollectionItrToOutMapper(ISetItemItrToOutMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<List<ScryfallSetOutEntity>> Map(IEnumerable<ISetItemItrEntity> setItems)
    {
        List<ScryfallSetOutEntity> results = [];

        foreach (ISetItemItrEntity setItem in setItems)
        {
            ScryfallSetOutEntity outEntity = await _mapper.Map(setItem).ConfigureAwait(false);
            results.Add(outEntity);
        }

        return results;
    }
}
