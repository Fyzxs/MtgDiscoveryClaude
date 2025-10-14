using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Domain.UserCards.Apis;
using Lib.MtgDiscovery.Entry.Entities.Outs.Cards;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.MtgDiscovery.Entry.Queries.Enrichments;

internal sealed class UserCardByArtistEnrichment : IUserCardByArtistEnrichment
{
    private readonly IUserCardsDomainService _userCardsDomainService;
    private readonly IUserCardCollectionEnrichmentApplier _enrichmentApplier;

    public UserCardByArtistEnrichment(ILogger logger) : this(
        new UserCardsDomainService(logger),
        new UserCardCollectionEnrichmentApplier())
    {
    }

    private UserCardByArtistEnrichment(
        IUserCardsDomainService userCardsDomainService,
        IUserCardCollectionEnrichmentApplier enrichmentApplier)
    {
        _userCardsDomainService = userCardsDomainService;
        _enrichmentApplier = enrichmentApplier;
    }

    public async Task Enrich(List<CardItemOutEntity> target, IUserCardsArtistItrEntity context)
    {
        IOperationResponse<IEnumerable<IUserCardOufEntity>> userCardResponse = await _userCardsDomainService.UserCardsByArtistAsync(context).ConfigureAwait(false);
        if (userCardResponse.IsFailure) return;

        await _enrichmentApplier.Apply(target, userCardResponse.ResponseData).ConfigureAwait(false);
    }
}
