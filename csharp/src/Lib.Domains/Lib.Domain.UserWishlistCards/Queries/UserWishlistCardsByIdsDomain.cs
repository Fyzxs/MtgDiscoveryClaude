using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Aggregator.UserWishlistCards.Apis;
using Lib.Shared.DataModels.Entities.Itrs.UserWishlistCards;
using Lib.Shared.DataModels.Entities.Oufs.UserWishlistCards;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Domain.UserWishlistCards.Queries;

internal sealed class UserWishlistCardsByIdsDomain : IUserWishlistCardsByIdsDomain
{
    private readonly IUserWishlistCardsQueryAggregatorService _userWishlistCardsAggregatorService;

    public UserWishlistCardsByIdsDomain(ILogger logger) : this(new UserWishlistCardsAggregatorService(logger))
    { }

    private UserWishlistCardsByIdsDomain(IUserWishlistCardsQueryAggregatorService userWishlistCardsAggregatorService) => _userWishlistCardsAggregatorService = userWishlistCardsAggregatorService;

    public async Task<IOperationResponse<IEnumerable<IUserWishlistCardOufEntity>>> Execute(IUserWishlistCardsByIdsItrEntity input, CancellationToken cancellationToken) => await _userWishlistCardsAggregatorService.UserWishlistCardsByIdsAsync(input, cancellationToken).ConfigureAwait(false);
}
