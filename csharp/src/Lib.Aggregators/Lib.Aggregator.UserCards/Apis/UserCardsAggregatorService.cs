using System.Collections.Generic;
using System.Threading;
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

    public async Task<IOperationResponse<IUserCardOufEntity>> AddUserCardOnlyAsync(
        IUserCardItrEntity userCard,
        CancellationToken cancellationToken)
        => await _commandOperations.AddUserCardOnlyAsync(userCard, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardAsync(
        IUserCardItrEntity userCard,
        CancellationToken cancellationToken)
        => await _queryOperations.UserCardAsync(userCard, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardsBySetAsync(
        IUserCardsSetItrEntity userCardsSet,
        CancellationToken cancellationToken)
        => await _queryOperations.UserCardsBySetAsync(userCardsSet, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardsByIdsAsync(
        IUserCardsByIdsItrEntity userCards,
        CancellationToken cancellationToken)
        => await _queryOperations.UserCardsByIdsAsync(userCards, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardsByArtistAsync(
        IUserCardsArtistItrEntity userCardsArtist,
        CancellationToken cancellationToken)
        => await _queryOperations.UserCardsByArtistAsync(userCardsArtist, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardsByNameAsync(
        IUserCardsNameItrEntity userCardsName,
        CancellationToken cancellationToken)
        => await _queryOperations.UserCardsByNameAsync(userCardsName, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<ISigningResultOufEntity>> UserCardsForSigningAsync(
        IUserCardsForSigningItrEntity userCardsForSigning,
        CancellationToken cancellationToken)
        => await _queryOperations.UserCardsForSigningAsync(userCardsForSigning, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<IUserCardOufEntity>> AddUserCardAsync(
        IUserCardItrEntity userCard,
        CancellationToken cancellationToken)
        => await _commandOperations.AddUserCardAsync(userCard, cancellationToken).ConfigureAwait(false);
}
