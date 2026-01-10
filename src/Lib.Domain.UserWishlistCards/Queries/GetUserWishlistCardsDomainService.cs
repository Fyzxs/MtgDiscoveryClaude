using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Aggregator.UserWishlistCards.Apis;
using Lib.Shared.DataModels.Entities.Itrs.UserWishlistCards;
using Lib.Shared.DataModels.Entities.Oufs.UserWishlistCards;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Domain.UserWishlistCards.Queries;

internal sealed class GetUserWishlistCardsDomainService : IGetUserWishlistCardsDomainService
{
    private readonly IUserWishlistCardsQueryAggregatorService _userWishlistCardsAggregatorService;

    public GetUserWishlistCardsDomainService(ILogger logger) : this(new UserWishlistCardsAggregatorService(logger))
    { }

    private GetUserWishlistCardsDomainService(IUserWishlistCardsQueryAggregatorService userWishlistCardsAggregatorService) => _userWishlistCardsAggregatorService = userWishlistCardsAggregatorService;

    public async Task<IOperationResponse<IEnumerable<IUserWishlistCardOufEntity>>> Execute(IUserWishlistCardsQueryItrEntity input) => await _userWishlistCardsAggregatorService.GetUserWishlistCardsAsync(input).ConfigureAwait(false);
}
