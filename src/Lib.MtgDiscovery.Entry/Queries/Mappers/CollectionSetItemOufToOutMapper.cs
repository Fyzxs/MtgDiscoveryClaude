using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Outs.Sets;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.MtgDiscovery.Entry.Queries.Mappers;

internal sealed class CollectionSetItemOufToOutMapper : ICollectionSetItemOufToOutMapper
{
    private readonly ISetItemOufToOutMapper _mapper;

    public CollectionSetItemOufToOutMapper() : this(new SetItemOufToOutMapper())
    { }

    private CollectionSetItemOufToOutMapper(ISetItemOufToOutMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<List<ScryfallSetOutEntity>> Map(ISetItemCollectionOufEntity collection)
    {
        ScryfallSetOutEntity[] mappedSets = await Task.WhenAll(
            collection.Data.Select(setItem => _mapper.Map(setItem))
        ).ConfigureAwait(false);

        return mappedSets.ToList();
    }
}
