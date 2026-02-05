using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Domain.UserWishlistCards.Apis;
using Lib.MtgDiscovery.Entry.Entities.Outs.Cards;
using Lib.MtgDiscovery.Entry.Queries.Actions.Integrators;
using Lib.Shared.DataModels.Entities.Args.User;
using Lib.Shared.DataModels.Entities.Itrs.UserWishlistCards;
using Lib.Shared.DataModels.Entities.Oufs.UserWishlistCards;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Enrichments;

internal sealed class UserWishlistCardByIdsEnrichment : IUserWishlistCardByIdsEnrichment
{
    private readonly IUserWishlistCardsDomainService _userWishlistCardsDomainService;
    private readonly IUserWishlistCardIntegrator _integrator;

    public UserWishlistCardByIdsEnrichment(ILogger logger) : this(
        new UserWishlistCardsDomainService(logger),
        new UserWishlistCardIntegrator())
    {
    }

    private UserWishlistCardByIdsEnrichment(
        IUserWishlistCardsDomainService userWishlistCardsDomainService,
        IUserWishlistCardIntegrator integrator)
    {
        _userWishlistCardsDomainService = userWishlistCardsDomainService;
        _integrator = integrator;
    }

    public async Task Enrich(List<CardItemOutEntity> target, IUserIdArgEntity args, CancellationToken cancellationToken)
    {
        if (args.DoesNotHaveUserId)
            return;

        // Use single efficient query to get ALL wishlist cards for user, then filter client-side
        // This is much more efficient than individual point reads per card ID
        IUserWishlistCardsQueryItrEntity queryItr = new UserWishlistCardsQueryItrEntity { UserId = args.UserId };

        IOperationResponse<IEnumerable<IUserWishlistCardOufEntity>> userWishlistCardResponse = await _userWishlistCardsDomainService.GetUserWishlistCardsAsync(queryItr, cancellationToken).ConfigureAwait(false);
        if (userWishlistCardResponse.IsFailure)
            return;

        _ = await _integrator.Integrate(target, userWishlistCardResponse.ResponseData).ConfigureAwait(false);
    }

    private sealed class UserWishlistCardsQueryItrEntity : IUserWishlistCardsQueryItrEntity
    {
        public required string UserId { get; init; }
    }
}
