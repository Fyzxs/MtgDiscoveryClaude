using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.Aggregator.Sets.Queries.Mappers;

/// <summary>
/// Maps collections of ScryfallSetItemExtEntity to ISetItemItrEntity.
/// </summary>
internal sealed class CollectionSetItemExtToItrMapper : ICollectionSetItemExtToItrMapper
{
    private readonly ISetItemExtToItrMapper _mapper;

    public CollectionSetItemExtToItrMapper() : this(new SetItemExtToItrMapper())
    { }

    private CollectionSetItemExtToItrMapper(ISetItemExtToItrMapper mapper) => _mapper = mapper;

    public async Task<IEnumerable<ISetItemItrEntity>> Map([NotNull] IEnumerable<ScryfallSetItemExtEntity> source)
    {
        IEnumerable<Task<ISetItemItrEntity>> tasks = source.Select(item => _mapper.Map(item));
        ISetItemItrEntity[] results = await Task.WhenAll(tasks).ConfigureAwait(false);
        return results;
    }
}
