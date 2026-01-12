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

    public Task<IOperationResponse<IUserWishlistCardOufEntity>> AddUserWishlistCardAsync(IUserWishlistCardItrEntity wishlistCard) => _commandOperations.AddUserWishlistCardAsync(wishlistCard);

    public Task<IOperationResponse<IUserWishlistCardOufEntity>> RemoveUserWishlistCardAsync(IUserWishlistCardItrEntity wishlistCard) => _commandOperations.RemoveUserWishlistCardAsync(wishlistCard);

    public Task<IOperationResponse<IEnumerable<IUserWishlistCardOufEntity>>> GetUserWishlistCardsAsync(IUserWishlistCardsQueryItrEntity query) => _queryOperations.GetUserWishlistCardsAsync(query);

    public Task<IOperationResponse<IEnumerable<IUserWishlistCardOufEntity>>> UserWishlistCardsByIdsAsync(IUserWishlistCardsByIdsItrEntity userWishlistCards) => _queryOperations.UserWishlistCardsByIdsAsync(userWishlistCards);

    public Task<IOperationResponse<IEnumerable<IUserWishlistCardOufEntity>>> UserWishlistCardsBySetAsync(IUserWishlistCardsSetItrEntity userWishlistCardsSet) => _queryOperations.UserWishlistCardsBySetAsync(userWishlistCardsSet);
}
