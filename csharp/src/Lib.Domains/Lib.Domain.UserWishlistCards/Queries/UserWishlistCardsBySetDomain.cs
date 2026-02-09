using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Aggregator.UserWishlistCards.Apis;
using Lib.Shared.DataModels.Entities.Itrs.UserWishlistCards;
using Lib.Shared.DataModels.Entities.Oufs.UserWishlistCards;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Domain.UserWishlistCards.Queries;

internal sealed class UserWishlistCardsBySetDomain : IUserWishlistCardsBySetDomain
{
    private readonly IUserWishlistCardsQueryAggregatorService _userWishlistCardsAggregatorService;

    public UserWishlistCardsBySetDomain(ILogger logger) : this(new UserWishlistCardsAggregatorService(logger))
    { }

    private UserWishlistCardsBySetDomain(IUserWishlistCardsQueryAggregatorService userWishlistCardsAggregatorService) => _userWishlistCardsAggregatorService = userWishlistCardsAggregatorService;

    public async Task<IOperationResponse<IEnumerable<IUserWishlistCardOufEntity>>> Execute(IUserWishlistCardsSetItrEntity input, CancellationToken cancellationToken) => await _userWishlistCardsAggregatorService.UserWishlistCardsBySetAsync(input, cancellationToken).ConfigureAwait(false);
}
