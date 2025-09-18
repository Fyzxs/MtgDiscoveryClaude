using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Aggregator.UserCards.Commands.Mappers;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.Aggregator.UserCards.Queries.Mappers;

/// <summary>
/// Maps collections of UserCardExtEntity to IUserCardItrEntity.
/// </summary>
internal sealed class CollectionUserCardExtToItrMapper : ICollectionUserCardExtToItrMapper
{
    private readonly IUserCardExtToItrEntityMapper _singleMapper;

    public CollectionUserCardExtToItrMapper() : this(new UserCardExtToItrEntityMapper())
    { }

    private CollectionUserCardExtToItrMapper(IUserCardExtToItrEntityMapper singleMapper)
    {
        _singleMapper = singleMapper;
    }

    public async Task<IEnumerable<IUserCardItrEntity>> Map([NotNull] IEnumerable<UserCardExtEntity> source)
    {
        List<Task<IUserCardItrEntity>> tasks = [.. source.Select(item => _singleMapper.Map(item))];
        IUserCardItrEntity[] results = await Task.WhenAll(tasks).ConfigureAwait(false);
        return results;
    }
}
