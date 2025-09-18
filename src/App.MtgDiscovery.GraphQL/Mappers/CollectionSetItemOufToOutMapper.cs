using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.MtgDiscovery.GraphQL.Entities.Outs.Sets;
using Lib.Shared.DataModels.Entities.Itrs;

namespace App.MtgDiscovery.GraphQL.Mappers;

internal sealed class CollectionSetItemOufToOutMapper : ICollectionSetItemOufToOutMapper
{
    private readonly ISetItemOufToOutMapper _mapper;

    public CollectionSetItemOufToOutMapper() : this(new SetItemOufToOutMapper())
    { }

    private CollectionSetItemOufToOutMapper(ISetItemOufToOutMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<ICollection<ScryfallSetOutEntity>> Map(IEnumerable<ISetItemItrEntity> setItems)
    {
        ScryfallSetOutEntity[] mappedSets = await Task.WhenAll(
            setItems.Select(setItem => _mapper.Map(setItem))
        ).ConfigureAwait(false);

        return mappedSets;
    }
}
