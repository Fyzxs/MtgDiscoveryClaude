using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Aggregator.UserCards.Commands;
using Lib.Aggregator.UserCards.Queries;
using Lib.Shared.DataModels.Entities.Itrs.UserCards;
using Lib.Shared.DataModels.Entities.Oufs.UserCards;
using Lib.Shared.DataModels.Entities.Oufs.UserCards.Signing;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.UserCards.Apis;

public sealed class UserCardsAggregatorService : IUserCardsAggregatorService
{
    private readonly IUserCardsCommandAggregatorService _commandOperations;
    private readonly IUserCardsQueryAggregatorService _queryOperations;

    public UserCardsAggregatorService(ILogger logger) : this(
        new UserCardsCommandAggregator(logger),
        new UserCardsQueryAggregator(logger))
    { }

    private UserCardsAggregatorService(
        IUserCardsCommandAggregatorService commandOperations,
        IUserCardsQueryAggregatorService queryOperations)
    {
        _commandOperations = commandOperations;
        _queryOperations = queryOperations;
    }

    public async Task<IOperationResponse<IUserCardOufEntity>> AddUserCardOnlyAsync(IUserCardItrEntity userCard) => await _commandOperations.AddUserCardOnlyAsync(userCard);

    public async Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardAsync(IUserCardItrEntity userCard) => await _queryOperations.UserCardAsync(userCard);

    public async Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardsBySetAsync(IUserCardsSetItrEntity userCardsSet) => await _queryOperations.UserCardsBySetAsync(userCardsSet);

    public async Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardsByIdsAsync(IUserCardsByIdsItrEntity userCards) => await _queryOperations.UserCardsByIdsAsync(userCards);

    public async Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardsByArtistAsync(IUserCardsArtistItrEntity userCardsArtist) => await _queryOperations.UserCardsByArtistAsync(userCardsArtist);

    public async Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardsByNameAsync(IUserCardsNameItrEntity userCardsName) => await _queryOperations.UserCardsByNameAsync(userCardsName);

    public async Task<IOperationResponse<ISigningResultOufEntity>> UserCardsForSigningAsync(IUserCardsForSigningItrEntity userCardsForSigning) => await _queryOperations.UserCardsForSigningAsync(userCardsForSigning);

    public async Task<IOperationResponse<IUserCardOufEntity>> AddUserCardAsync(IUserCardItrEntity userCard) => await _commandOperations.AddUserCardAsync(userCard);
}
