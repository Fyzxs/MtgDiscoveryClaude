using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Domain.UserCards.Apis;
using Lib.MtgDiscovery.Entry.Entities.Outs.Cards;
using Lib.MtgDiscovery.Entry.Queries.Mappers;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.MtgDiscovery.Entry.Queries.Enrichments;

internal sealed class UserCardByIdsEnrichment : IUserCardByIdsEnrichment
{
    private readonly IUserCardsDomainService _userCardsDomainService;
    private readonly ICollectionCardItemToByIdsItrMapper _collectionCardItemToByIdsItrMapper;
    private readonly IUserCardCollectionEnrichmentApplier _enrichmentApplier;

    public UserCardByIdsEnrichment(ILogger logger) : this(
        new UserCardsDomainService(logger),
        new CollectionCardItemToByIdsItrMapper(),
        new UserCardCollectionEnrichmentApplier())
    {
    }

    private UserCardByIdsEnrichment(
        IUserCardsDomainService userCardsDomainService,
        ICollectionCardItemToByIdsItrMapper collectionCardItemToByIdsItrMapper,
        IUserCardCollectionEnrichmentApplier enrichmentApplier)
    {
        _userCardsDomainService = userCardsDomainService;
        _collectionCardItemToByIdsItrMapper = collectionCardItemToByIdsItrMapper;
        _enrichmentApplier = enrichmentApplier;
    }

    public async Task Enrich(List<CardItemOutEntity> target, IUserIdArgEntity args)
    {
        if (args.DoesNotHaveUserId) return;

        IUserCardsByIdsItrEntity userCardsByIdsItrEntity = await _collectionCardItemToByIdsItrMapper.Map(target, args).ConfigureAwait(false);

        IOperationResponse<IEnumerable<IUserCardOufEntity>> userCardResponse = await _userCardsDomainService.UserCardsByIdsAsync(userCardsByIdsItrEntity).ConfigureAwait(false);
        if (userCardResponse.IsFailure) return;

        await _enrichmentApplier.Apply(target, userCardResponse.ResponseData).ConfigureAwait(false);
    }
}
