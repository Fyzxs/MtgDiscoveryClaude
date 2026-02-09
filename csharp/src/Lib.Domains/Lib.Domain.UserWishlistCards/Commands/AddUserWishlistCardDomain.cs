using System.Threading;
using System.Threading.Tasks;
using Lib.Aggregator.UserWishlistCards.Apis;
using Lib.Shared.DataModels.Entities.Itrs.UserWishlistCards;
using Lib.Shared.DataModels.Entities.Oufs.UserWishlistCards;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Domain.UserWishlistCards.Commands;

internal sealed class AddUserWishlistCardDomain : IAddUserWishlistCardDomain
{
    private readonly IUserWishlistCardsCommandAggregatorService _userWishlistCardsAggregatorService;

    public AddUserWishlistCardDomain(ILogger logger) : this(new UserWishlistCardsAggregatorService(logger))
    { }

    private AddUserWishlistCardDomain(IUserWishlistCardsCommandAggregatorService userWishlistCardsAggregatorService) => _userWishlistCardsAggregatorService = userWishlistCardsAggregatorService;

    public async Task<IOperationResponse<IUserWishlistCardOufEntity>> Execute(IUserWishlistCardItrEntity input, CancellationToken cancellationToken) => await _userWishlistCardsAggregatorService.AddUserWishlistCardAsync(input, cancellationToken).ConfigureAwait(false);
}
