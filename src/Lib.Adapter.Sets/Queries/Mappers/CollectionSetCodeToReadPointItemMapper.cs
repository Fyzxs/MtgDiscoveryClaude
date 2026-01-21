using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Cosmos.Apis.Operators;

namespace Lib.Adapter.Sets.Queries.Mappers;

internal sealed class CollectionSetCodeToReadPointItemMapper : ICollectionSetCodeToReadPointItemMapper
{
    private readonly IStringCollectionToReadPointItemMapper _mapper;

    public CollectionSetCodeToReadPointItemMapper() : this(new StringCollectionToReadPointItemMapper())
    {
    }

    private CollectionSetCodeToReadPointItemMapper(IStringCollectionToReadPointItemMapper mapper) =>
        _mapper = mapper;

    public Task<ICollection<ReadPointItem>> Map(IEnumerable<string> setCodes) =>
        _mapper.Map(setCodes);
}
