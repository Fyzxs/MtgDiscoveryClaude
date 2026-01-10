using System.Threading.Tasks;
using Lib.Aggregator.UserWishlistCards.Apis;
using Lib.Shared.DataModels.Entities.Itrs.UserWishlistCards;
using Lib.Shared.DataModels.Entities.Oufs.UserWishlistCards;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Domain.UserWishlistCards.Commands;

internal sealed class RemoveUserWishlistCardDomainService : IRemoveUserWishlistCardDomainService
{
    private readonly IUserWishlistCardsCommandAggregatorService _userWishlistCardsAggregatorService;

    public RemoveUserWishlistCardDomainService(ILogger logger) : this(new UserWishlistCardsAggregatorService(logger))
    { }

    private RemoveUserWishlistCardDomainService(IUserWishlistCardsCommandAggregatorService userWishlistCardsAggregatorService) => _userWishlistCardsAggregatorService = userWishlistCardsAggregatorService;

    public async Task<IOperationResponse<IUserWishlistCardOufEntity>> Execute(IUserWishlistCardItrEntity input) => await _userWishlistCardsAggregatorService.RemoveUserWishlistCardAsync(input).ConfigureAwait(false);
}
