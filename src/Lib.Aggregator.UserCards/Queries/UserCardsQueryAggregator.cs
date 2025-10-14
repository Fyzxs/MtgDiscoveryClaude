using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Aggregator.UserCards.Apis;
using Lib.Aggregator.UserCards.Queries.UserCard;
using Lib.Aggregator.UserCards.Queries.UserCardsByArtist;
using Lib.Aggregator.UserCards.Queries.UserCardsByIds;
using Lib.Aggregator.UserCards.Queries.UserCardsByName;
using Lib.Aggregator.UserCards.Queries.UserCardsBySet;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.UserCards.Queries;

internal sealed class UserCardsQueryAggregator : IUserCardsAggregatorService
{
    private readonly IUserCardAggregatorService _userCardOperations;
    private readonly IUserCardsBySetAggregatorService _userCardsBySetOperations;
    private readonly IUserCardsByIdsAggregatorService _userCardsByIdsOperations;
    private readonly IUserCardsByArtistAggregatorService _userCardsByArtistOperations;
    private readonly IUserCardsByNameAggregatorService _userCardsByNameOperations;

    public UserCardsQueryAggregator(ILogger logger) : this(
        new UserCardAggregatorService(logger),
        new UserCardsBySetAggregatorService(logger),
        new UserCardsByIdsAggregatorService(logger),
        new UserCardsByArtistAggregatorService(logger),
        new UserCardsByNameAggregatorService(logger))
    { }

    private UserCardsQueryAggregator(
        IUserCardAggregatorService userCardOperations,
        IUserCardsBySetAggregatorService userCardsBySetOperations,
        IUserCardsByIdsAggregatorService userCardsByIdsOperations,
        IUserCardsByArtistAggregatorService userCardsByArtistOperations,
        IUserCardsByNameAggregatorService userCardsByNameOperations)
    {
        _userCardOperations = userCardOperations;
        _userCardsBySetOperations = userCardsBySetOperations;
        _userCardsByIdsOperations = userCardsByIdsOperations;
        _userCardsByArtistOperations = userCardsByArtistOperations;
        _userCardsByNameOperations = userCardsByNameOperations;
    }

    public Task<IOperationResponse<IUserCardOufEntity>> AddUserCardAsync(IUserCardItrEntity userCard) => throw new System.NotImplementedException("Use UserCardsCommandAggregator");

    public Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardAsync(IUserCardItrEntity userCard) => _userCardOperations.UserCardAsync(userCard);

    public Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardsBySetAsync(IUserCardsSetItrEntity userCardsSet) => _userCardsBySetOperations.UserCardsBySetAsync(userCardsSet);

    public Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardsByIdsAsync(IUserCardsByIdsItrEntity userCards) => _userCardsByIdsOperations.UserCardsByIdsAsync(userCards);

    public Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardsByArtistAsync(IUserCardsArtistItrEntity userCardsArtist) => _userCardsByArtistOperations.UserCardsByArtistAsync(userCardsArtist);

    public Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardsByNameAsync(IUserCardsNameItrEntity userCardsName) => _userCardsByNameOperations.UserCardsByNameAsync(userCardsName);
}
