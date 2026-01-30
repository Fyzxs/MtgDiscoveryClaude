using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Domain.UserSetCards.Apis;
using Lib.MtgDiscovery.Entry.Entities.Outs.Sets;
using Lib.MtgDiscovery.Entry.Queries.Actions.Integrators;
using Lib.MtgDiscovery.Entry.Queries.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Args.User;
using Lib.Shared.DataModels.Entities.Itrs.UserSetCards;
using Lib.Shared.DataModels.Entities.Oufs.UserSetCards;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Enrichments;

internal sealed class UserSetEnrichment : IUserSetEnrichment
{
    private readonly IUserSetCardsDomainService _userSetCardsDomainService;
    private readonly IUserSetCollectionIntegrator _integrator;
    private readonly IUserIdArgToAllUserSetCardsItrMapper _userIdArgToAllUserSetCardsItrMapper;

    public UserSetEnrichment(ILogger logger) : this(
        new UserSetCardsDomainService(logger),
        new UserSetCollectionIntegrator(),
        new UserIdArgToAllUserSetCardsItrMapper())
    {
    }

    private UserSetEnrichment(
        IUserSetCardsDomainService userSetCardsDomainService,
        IUserSetCollectionIntegrator integrator,
        IUserIdArgToAllUserSetCardsItrMapper userIdArgToAllUserSetCardsItrMapper)
    {
        _userSetCardsDomainService = userSetCardsDomainService;
        _integrator = integrator;
        _userIdArgToAllUserSetCardsItrMapper = userIdArgToAllUserSetCardsItrMapper;
    }

    public async Task Enrich(List<SetItemOutEntity> outEntities, IUserIdArgEntity args)
    {
        IAllUserSetCardsItrEntity itrEntity = await _userIdArgToAllUserSetCardsItrMapper.Map(args).ConfigureAwait(false);
        IOperationResponse<IEnumerable<IUserSetCardOufEntity>> userSetCardResponse = await _userSetCardsDomainService.AllUserSetCardsAsync(itrEntity).ConfigureAwait(false);

        if (userSetCardResponse.IsFailure) return;

        _ = await _integrator.Integrate(outEntities, userSetCardResponse.ResponseData).ConfigureAwait(false);
    }
}
