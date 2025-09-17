using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Shared.DataModels.Entities;

namespace Lib.Aggregator.Sets.Queries.Mappers;

/// <summary>
/// Maps collections of ScryfallSetItemExtEntity to ISetItemItrEntity.
/// </summary>
internal sealed class CollectionSetItemExtToItrMapper : ICollectionSetItemExtToItrMapper
{
    private readonly ISetItemExtToItrMapper _singleMapper;

    public CollectionSetItemExtToItrMapper() : this(new SetItemExtToItrMapper())
    { }

    private CollectionSetItemExtToItrMapper(ISetItemExtToItrMapper singleMapper)
    {
        _singleMapper = singleMapper;
    }

    public async Task<IEnumerable<ISetItemItrEntity>> Map([NotNull] IEnumerable<ScryfallSetItemExtEntity> source)
    {
        List<Task<ISetItemItrEntity>> tasks = [.. source.Select(item => _singleMapper.Map(item))];
        ISetItemItrEntity[] results = await Task.WhenAll(tasks).ConfigureAwait(false);
        return results;
    }
}
