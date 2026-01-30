using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Aggregator.UserWishlistCards.Commands.Mappers;
using Lib.Shared.DataModels.Entities.Oufs.UserWishlistCards;

namespace Lib.Aggregator.UserWishlistCards.Queries.Mappers;

/// <summary>
/// Maps collections of UserWishlistCardExtEntity to IUserWishlistCardOufEntity.
/// </summary>
internal sealed class CollectionUserWishlistCardExtToOufMapper : ICollectionUserWishlistCardExtToOufMapper
{
    private readonly IUserWishlistCardExtToOufEntityMapper _mapper;

    public CollectionUserWishlistCardExtToOufMapper() : this(new UserWishlistCardExtToOufEntityMapper())
    { }

    private CollectionUserWishlistCardExtToOufMapper(IUserWishlistCardExtToOufEntityMapper mapper) => _mapper = mapper;

    public async Task<IEnumerable<IUserWishlistCardOufEntity>> Map([NotNull] IEnumerable<UserWishlistCardExtEntity> source)
    {
        List<Task<IUserWishlistCardOufEntity>> tasks = [.. source.Select(item => _mapper.Map(item))];
        IUserWishlistCardOufEntity[] results = await Task.WhenAll(tasks).ConfigureAwait(false);
        return results;
    }
}
