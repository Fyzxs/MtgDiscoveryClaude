using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Aggregator.UserCards.Commands.Mappers;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.Aggregator.UserCards.Queries.Mappers;

/// <summary>
/// Maps collections of UserCardExtEntity to IUserCardItrEntity.
/// </summary>
internal sealed class CollectionUserCardExtToItrMapper : ICollectionUserCardExtToItrMapper
{
    private readonly IUserCardExtToItrEntityMapper _mapper;

    public CollectionUserCardExtToItrMapper() : this(new UserCardExtToItrEntityMapper())
    { }

    private CollectionUserCardExtToItrMapper(IUserCardExtToItrEntityMapper mapper) => _mapper = mapper;

    public async Task<IEnumerable<IUserCardOufEntity>> Map([NotNull] IEnumerable<UserCardExtEntity> source)
    {
        List<Task<IUserCardOufEntity>> tasks = [.. source.Select(item => _mapper.Map(item))];
        IUserCardOufEntity[] results = await Task.WhenAll(tasks).ConfigureAwait(false);
        return results;
    }
}
