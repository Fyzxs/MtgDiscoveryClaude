using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Aggregator.UserCards.Apis;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.UserCards.Queries;

internal sealed class UserCardsQueryAggregator : IUserCardsQueryAggregatorService
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

    public Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardAsync(IUserCardItrEntity userCard) => _userCardOperations.Execute(userCard);

    public Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardsBySetAsync(IUserCardsSetItrEntity userCardsSet) => _userCardsBySetOperations.Execute(userCardsSet);

    public Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardsByIdsAsync(IUserCardsByIdsItrEntity userCards) => _userCardsByIdsOperations.Execute(userCards);

    public Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardsByArtistAsync(IUserCardsArtistItrEntity userCardsArtist) => _userCardsByArtistOperations.Execute(userCardsArtist);

    public Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardsByNameAsync(IUserCardsNameItrEntity userCardsName) => _userCardsByNameOperations.Execute(userCardsName);
}
