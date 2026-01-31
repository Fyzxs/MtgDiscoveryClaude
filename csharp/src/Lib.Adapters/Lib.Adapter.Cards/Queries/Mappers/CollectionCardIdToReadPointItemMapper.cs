using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Cosmos.Apis.Operators;

namespace Lib.Adapter.Cards.Queries.Mappers;

internal sealed class CollectionCardIdToReadPointItemMapper : ICollectionCardIdToReadPointItemMapper
{
    private readonly IStringCollectionToReadPointItemMapper _mapper;

    public CollectionCardIdToReadPointItemMapper() : this(new StringCollectionToReadPointItemMapper())
    {
    }

    private CollectionCardIdToReadPointItemMapper(IStringCollectionToReadPointItemMapper mapper) => _mapper = mapper;

    public async Task<ICollection<ReadPointItem>> Map(IEnumerable<string> cardIds) => await _mapper.Map(cardIds);
}
