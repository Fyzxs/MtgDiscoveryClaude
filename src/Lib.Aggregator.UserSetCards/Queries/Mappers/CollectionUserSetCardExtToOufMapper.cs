using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Shared.DataModels.Entities.Itrs.UserSetCards;

namespace Lib.Aggregator.UserSetCards.Queries.Mappers;

internal interface ICollectionUserSetCardExtToOufMapper
{
    Task<IEnumerable<IUserSetCardOufEntity>> Map([NotNull] IEnumerable<UserSetCardExtEntity> source);
}

internal sealed class CollectionUserSetCardExtToOufMapper : ICollectionUserSetCardExtToOufMapper
{
    private readonly IUserSetCardExtToItrMapper _mapper;

    public CollectionUserSetCardExtToOufMapper() : this(new UserSetCardExtToItrMapper())
    {
    }

    private CollectionUserSetCardExtToOufMapper(IUserSetCardExtToItrMapper mapper) => _mapper = mapper;

    public async Task<IEnumerable<IUserSetCardOufEntity>> Map([NotNull] IEnumerable<UserSetCardExtEntity> source)
    {
        List<Task<IUserSetCardOufEntity>> tasks = [.. source.Select(item => _mapper.Map(item))];
        IUserSetCardOufEntity[] results = await Task.WhenAll(tasks).ConfigureAwait(false);
        return results;
    }
}
