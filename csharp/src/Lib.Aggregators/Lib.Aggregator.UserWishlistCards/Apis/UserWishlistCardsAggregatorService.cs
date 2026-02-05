using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Aggregator.UserWishlistCards.Commands;
using Lib.Aggregator.UserWishlistCards.Queries;
using Lib.Shared.DataModels.Entities.Itrs.UserWishlistCards;
using Lib.Shared.DataModels.Entities.Oufs.UserWishlistCards;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.UserWishlistCards.Apis;

public sealed class UserWishlistCardsAggregatorService : IUserWishlistCardsAggregatorService
{
    private readonly IUserWishlistCardsCommandAggregatorService _commandOperations;
    private readonly IUserWishlistCardsQueryAggregatorService _queryOperations;

    public UserWishlistCardsAggregatorService(ILogger logger) : this(
        new UserWishlistCardsCommandAggregator(logger),
        new UserWishlistCardsQueryAggregator(logger))
    { }

    private UserWishlistCardsAggregatorService(
        IUserWishlistCardsCommandAggregatorService commandOperations,
        IUserWishlistCardsQueryAggregatorService queryOperations)
    {
        _commandOperations = commandOperations;
        _queryOperations = queryOperations;
    }

    public async Task<IOperationResponse<IUserWishlistCardOufEntity>> AddUserWishlistCardAsync(IUserWishlistCardItrEntity wishlistCard, CancellationToken cancellationToken) => await _commandOperations.AddUserWishlistCardAsync(wishlistCard, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<IUserWishlistCardOufEntity>> RemoveUserWishlistCardAsync(IUserWishlistCardItrEntity wishlistCard, CancellationToken cancellationToken) => await _commandOperations.RemoveUserWishlistCardAsync(wishlistCard, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<IUserWishlistCardOufEntity>>> GetUserWishlistCardsAsync(IUserWishlistCardsQueryItrEntity query, CancellationToken cancellationToken) => await _queryOperations.GetUserWishlistCardsAsync(query, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<IUserWishlistCardOufEntity>>> UserWishlistCardsByIdsAsync(IUserWishlistCardsByIdsItrEntity userWishlistCards, CancellationToken cancellationToken) => await _queryOperations.UserWishlistCardsByIdsAsync(userWishlistCards, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<IUserWishlistCardOufEntity>>> UserWishlistCardsBySetAsync(IUserWishlistCardsSetItrEntity userWishlistCardsSet, CancellationToken cancellationToken) => await _queryOperations.UserWishlistCardsBySetAsync(userWishlistCardsSet, cancellationToken).ConfigureAwait(false);
}
