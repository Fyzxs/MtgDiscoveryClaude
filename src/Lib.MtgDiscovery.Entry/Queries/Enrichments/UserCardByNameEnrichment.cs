using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Domain.UserCards.Apis;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.DataModels.Entities.Outs.Cards;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.MtgDiscovery.Entry.Queries.Enrichments;

internal sealed class UserCardByNameEnrichment : IUserCardByNameEnrichment
{
    private readonly IUserCardsDomainService _userCardsDomainService;
    private readonly IUserCardCollectionEnrichmentApplier _enrichmentApplier;

    public UserCardByNameEnrichment(ILogger logger) : this(
        new UserCardsDomainService(logger),
        new UserCardCollectionEnrichmentApplier())
    {
    }

    private UserCardByNameEnrichment(
        IUserCardsDomainService userCardsDomainService,
        IUserCardCollectionEnrichmentApplier enrichmentApplier)
    {
        _userCardsDomainService = userCardsDomainService;
        _enrichmentApplier = enrichmentApplier;
    }

    public async Task Enrich(List<CardItemOutEntity> target, IUserCardsNameItrEntity context)
    {
        IOperationResponse<IEnumerable<IUserCardOufEntity>> userCardResponse = await _userCardsDomainService.UserCardsByNameAsync(context).ConfigureAwait(false);
        if (userCardResponse.IsFailure) return;

        await _enrichmentApplier.Apply(target, userCardResponse.ResponseData).ConfigureAwait(false);
    }
}
