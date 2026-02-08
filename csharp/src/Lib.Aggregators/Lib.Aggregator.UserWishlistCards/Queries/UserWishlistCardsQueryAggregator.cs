using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Lib.Aggregator.UserWishlistCards.Apis;
using Lib.Aggregator.UserWishlistCards.Queries.Entities;
using Lib.Aggregator.UserWishlistCards.Queries.UserWishlistCardsByIds;
using Lib.Shared.DataModels.Entities.Itrs.UserWishlistCards;
using Lib.Shared.DataModels.Entities.Oufs.UserWishlistCards;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.UserWishlistCards.Queries;

internal sealed class UserWishlistCardsQueryAggregator : IUserWishlistCardsQueryAggregatorService
{
    private readonly IGetUserWishlistCardsAggregator _getUserWishlistCardsOperations;
    private readonly IUserWishlistCardsByIdsAggregatorService _userWishlistCardsByIdsAggregatorService;

    public UserWishlistCardsQueryAggregator(ILogger logger) : this(
        new GetUserWishlistCardsAggregator(logger),
        new UserWishlistCardsByIdsAggregatorService(logger))
    { }

    private UserWishlistCardsQueryAggregator(
        IGetUserWishlistCardsAggregator getUserWishlistCardsOperations,
        IUserWishlistCardsByIdsAggregatorService userWishlistCardsByIdsAggregatorService)
    {
        _getUserWishlistCardsOperations = getUserWishlistCardsOperations;
        _userWishlistCardsByIdsAggregatorService = userWishlistCardsByIdsAggregatorService;
    }

    public async Task<IOperationResponse<IEnumerable<IUserWishlistCardOufEntity>>> GetUserWishlistCardsAsync(IUserWishlistCardsQueryItrEntity query, CancellationToken cancellationToken) => await _getUserWishlistCardsOperations.Execute(query, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<IUserWishlistCardOufEntity>>> UserWishlistCardsByIdsAsync(IUserWishlistCardsByIdsItrEntity userWishlistCards, CancellationToken cancellationToken) => await _userWishlistCardsByIdsAggregatorService.Execute(userWishlistCards, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<IUserWishlistCardOufEntity>>> UserWishlistCardsBySetAsync(IUserWishlistCardsSetItrEntity userWishlistCardsSet, CancellationToken cancellationToken)
    {
        UserWishlistCardsQueryItrEntity query = new() { UserId = userWishlistCardsSet.UserId };
        IOperationResponse<IEnumerable<IUserWishlistCardOufEntity>> response = await _getUserWishlistCardsOperations.Execute(query, cancellationToken).ConfigureAwait(false);

        if (response.IsFailure)
        {
            return response;
        }

        if (string.IsNullOrEmpty(userWishlistCardsSet.SetId))
        {
            return response;
        }

        IEnumerable<IUserWishlistCardOufEntity> filteredBySet = response.ResponseData.Where(card => card.SetId == userWishlistCardsSet.SetId);
        return new SuccessOperationResponse<IEnumerable<IUserWishlistCardOufEntity>>(filteredBySet);
    }
}
