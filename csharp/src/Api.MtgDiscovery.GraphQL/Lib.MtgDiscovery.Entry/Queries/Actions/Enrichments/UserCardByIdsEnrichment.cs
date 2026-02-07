using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Domain.UserCards.Apis;
using Lib.MtgDiscovery.Entry.Entities.Outs.Cards;
using Lib.MtgDiscovery.Entry.Queries.Actions.Integrators;
using Lib.MtgDiscovery.Entry.Queries.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Args.User;
using Lib.Shared.DataModels.Entities.Itrs.UserCards;
using Lib.Shared.DataModels.Entities.Oufs.UserCards;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Enrichments;

internal sealed class UserCardByIdsEnrichment : IUserCardByIdsEnrichment
{
    private readonly IUserCardsDomainService _userCardsDomainService;
    private readonly ICollectionCardItemToByIdsItrMapper _collectionCardItemToByIdsItrMapper;
    private readonly IUserCardCollectionIntegrator _integrator;

    public UserCardByIdsEnrichment(ILogger logger) : this(
        new UserCardsDomainService(logger),
        new CollectionCardItemToByIdsItrMapper(),
        new UserCardCollectionIntegrator())
    {
    }

    private UserCardByIdsEnrichment(
        IUserCardsDomainService userCardsDomainService,
        ICollectionCardItemToByIdsItrMapper collectionCardItemToByIdsItrMapper,
        IUserCardCollectionIntegrator integrator)
    {
        _userCardsDomainService = userCardsDomainService;
        _collectionCardItemToByIdsItrMapper = collectionCardItemToByIdsItrMapper;
        _integrator = integrator;
    }

    public async Task Enrich(List<CardItemOutEntity> target, IUserIdArgEntity args, CancellationToken cancellationToken)
    {
        if (args.DoesNotHaveUserId)
            return;

        IUserCardsByIdsItrEntity userCardsByIdsItrEntity = await _collectionCardItemToByIdsItrMapper.Map(target, args).ConfigureAwait(false);

        IOperationResponse<IEnumerable<IUserCardOufEntity>> userCardResponse = await _userCardsDomainService.UserCardsByIdsAsync(userCardsByIdsItrEntity, cancellationToken).ConfigureAwait(false);
        if (userCardResponse.IsFailure)
            return;

        _ = await _integrator.Integrate(target, userCardResponse.ResponseData).ConfigureAwait(false);
    }
}
