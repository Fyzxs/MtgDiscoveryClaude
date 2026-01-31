using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Cosmos.Apis.Operators;

namespace Lib.Adapter.Sets.Queries.Mappers;

internal sealed class CollectionSetIdToReadPointItemMapper : ICollectionSetIdToReadPointItemMapper
{
    private readonly IStringCollectionToReadPointItemMapper _mapper;

    public CollectionSetIdToReadPointItemMapper() : this(new StringCollectionToReadPointItemMapper())
    {
    }

    private CollectionSetIdToReadPointItemMapper(IStringCollectionToReadPointItemMapper mapper) => _mapper = mapper;

    public async Task<ICollection<ReadPointItem>> Map(IEnumerable<string> setIds) => await _mapper.Map(setIds);
}
