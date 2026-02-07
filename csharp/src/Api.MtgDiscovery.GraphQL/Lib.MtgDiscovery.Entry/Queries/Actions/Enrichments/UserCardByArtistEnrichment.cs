using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Domain.UserCards.Apis;
using Lib.MtgDiscovery.Entry.Entities.Outs.Cards;
using Lib.MtgDiscovery.Entry.Queries.Actions.Integrators;
using Lib.Shared.DataModels.Entities.Itrs.UserCards;
using Lib.Shared.DataModels.Entities.Oufs.UserCards;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Enrichments;

internal sealed class UserCardByArtistEnrichment : IUserCardByArtistEnrichment
{
    private readonly IUserCardsDomainService _userCardsDomainService;
    private readonly IUserCardCollectionIntegrator _integrator;

    public UserCardByArtistEnrichment(ILogger logger) : this(
        new UserCardsDomainService(logger),
        new UserCardCollectionIntegrator())
    {
    }

    private UserCardByArtistEnrichment(
        IUserCardsDomainService userCardsDomainService,
        IUserCardCollectionIntegrator integrator)
    {
        _userCardsDomainService = userCardsDomainService;
        _integrator = integrator;
    }

    public async Task Enrich(List<CardItemOutEntity> target, IUserCardsArtistItrEntity context, CancellationToken cancellationToken)
    {
        IOperationResponse<IEnumerable<IUserCardOufEntity>> userCardResponse = await _userCardsDomainService.UserCardsByArtistAsync(context, cancellationToken).ConfigureAwait(false);
        if (userCardResponse.IsFailure)
            return;

        _ = await _integrator.Integrate(target, userCardResponse.ResponseData).ConfigureAwait(false);
    }
}
