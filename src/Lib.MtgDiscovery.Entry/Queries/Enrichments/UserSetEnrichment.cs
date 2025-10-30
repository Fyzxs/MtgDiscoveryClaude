using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Domain.UserSetCards.Apis;
using Lib.MtgDiscovery.Entry.Entities.Outs.Sets;
using Lib.MtgDiscovery.Entry.Queries.Mappers;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs.UserSetCards;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.MtgDiscovery.Entry.Queries.Enrichments;

internal sealed class UserSetEnrichment : IUserSetEnrichment
{
    private readonly IUserSetCardsDomainService _userSetCardsDomainService;
    private readonly IUserSetCollectionEnrichmentApplier _enrichmentApplier;
    private readonly IUserIdArgToAllUserSetCardsItrMapper _userIdArgToAllUserSetCardsItrMapper;

    public UserSetEnrichment(ILogger logger) : this(
        new UserSetCardsDomainService(logger),
        new UserSetCollectionEnrichmentApplier(),
        new UserIdArgToAllUserSetCardsItrMapper())
    {
    }

    private UserSetEnrichment(
        IUserSetCardsDomainService userSetCardsDomainService,
        IUserSetCollectionEnrichmentApplier enrichmentApplier,
        IUserIdArgToAllUserSetCardsItrMapper userIdArgToAllUserSetCardsItrMapper)
    {
        _userSetCardsDomainService = userSetCardsDomainService;
        _enrichmentApplier = enrichmentApplier;
        _userIdArgToAllUserSetCardsItrMapper = userIdArgToAllUserSetCardsItrMapper;
    }

    public async Task Enrich(List<SetItemOutEntity> outEntities, IUserIdArgEntity args)
    {
        IAllUserSetCardsItrEntity itrEntity = await _userIdArgToAllUserSetCardsItrMapper.Map(args).ConfigureAwait(false);
        IOperationResponse<IEnumerable<IUserSetCardOufEntity>> userSetCardResponse =
            await _userSetCardsDomainService.AllUserSetCardsAsync(itrEntity).ConfigureAwait(false);

        if (userSetCardResponse.IsFailure) return;

        await _enrichmentApplier.Apply(outEntities, userSetCardResponse.ResponseData).ConfigureAwait(false);
    }
}
