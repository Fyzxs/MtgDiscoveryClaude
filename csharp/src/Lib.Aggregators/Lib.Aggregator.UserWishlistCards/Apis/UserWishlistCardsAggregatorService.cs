using System.Collections.Generic;
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

    public async Task<IOperationResponse<IUserWishlistCardOufEntity>> AddUserWishlistCardAsync(IUserWishlistCardItrEntity wishlistCard) => await _commandOperations.AddUserWishlistCardAsync(wishlistCard);

    public async Task<IOperationResponse<IUserWishlistCardOufEntity>> RemoveUserWishlistCardAsync(IUserWishlistCardItrEntity wishlistCard) => await _commandOperations.RemoveUserWishlistCardAsync(wishlistCard);

    public async Task<IOperationResponse<IEnumerable<IUserWishlistCardOufEntity>>> GetUserWishlistCardsAsync(IUserWishlistCardsQueryItrEntity query) => await _queryOperations.GetUserWishlistCardsAsync(query);

    public async Task<IOperationResponse<IEnumerable<IUserWishlistCardOufEntity>>> UserWishlistCardsByIdsAsync(IUserWishlistCardsByIdsItrEntity userWishlistCards) => await _queryOperations.UserWishlistCardsByIdsAsync(userWishlistCards);

    public async Task<IOperationResponse<IEnumerable<IUserWishlistCardOufEntity>>> UserWishlistCardsBySetAsync(IUserWishlistCardsSetItrEntity userWishlistCardsSet) => await _queryOperations.UserWishlistCardsBySetAsync(userWishlistCardsSet);
}
