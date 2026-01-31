using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Aggregator.UserWishlistCards.Apis;
using Lib.Shared.DataModels.Entities.Itrs.UserWishlistCards;
using Lib.Shared.DataModels.Entities.Oufs.UserWishlistCards;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Domain.UserWishlistCards.Queries;

/// <summary>
/// Single-method service for retrieving multiple user wishlist cards using batch point read operations.
/// Delegates to aggregator layer for data retrieval.
/// </summary>
internal sealed class UserWishlistCardsByIdsDomainService : IUserWishlistCardsByIdsDomainService
{
    private readonly IUserWishlistCardsQueryAggregatorService _userWishlistCardsAggregatorService;

    public UserWishlistCardsByIdsDomainService(ILogger logger) : this(new UserWishlistCardsAggregatorService(logger))
    { }

    private UserWishlistCardsByIdsDomainService(IUserWishlistCardsQueryAggregatorService userWishlistCardsAggregatorService) => _userWishlistCardsAggregatorService = userWishlistCardsAggregatorService;

    public async Task<IOperationResponse<IEnumerable<IUserWishlistCardOufEntity>>> Execute(IUserWishlistCardsByIdsItrEntity input) => await _userWishlistCardsAggregatorService.UserWishlistCardsByIdsAsync(input).ConfigureAwait(false);
}
