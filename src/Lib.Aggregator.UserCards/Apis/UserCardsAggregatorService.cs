using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Aggregator.UserCards.Commands;
using Lib.Aggregator.UserCards.Queries;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.UserCards.Apis;

public sealed class UserCardsAggregatorService : IUserCardsAggregatorService
{
    private readonly IUserCardsAggregatorService _commandOperations;
    private readonly IUserCardsAggregatorService _queryOperations;

    public UserCardsAggregatorService(ILogger logger) : this(
        new UserCardsCommandAggregator(logger),
        new UserCardsQueryAggregator(logger))
    { }

    private UserCardsAggregatorService(
        IUserCardsAggregatorService commandOperations,
        IUserCardsAggregatorService queryOperations)
    {
        _commandOperations = commandOperations;
        _queryOperations = queryOperations;
    }

    public Task<IOperationResponse<IUserCardOufEntity>> AddUserCardAsync(IUserCardItrEntity userCard) => _commandOperations.AddUserCardAsync(userCard);

    public Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardAsync(IUserCardItrEntity userCard) => _queryOperations.UserCardAsync(userCard);

    public Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardsBySetAsync(IUserCardsSetItrEntity userCardsSet) => _queryOperations.UserCardsBySetAsync(userCardsSet);

    public Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardsByIdsAsync(IUserCardsByIdsItrEntity userCards) => _queryOperations.UserCardsByIdsAsync(userCards);

    public Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardsByArtistAsync(IUserCardsArtistItrEntity userCardsArtist) => _queryOperations.UserCardsByArtistAsync(userCardsArtist);

    public Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardsByNameAsync(IUserCardsNameItrEntity userCardsName) => _queryOperations.UserCardsByNameAsync(userCardsName);
}
