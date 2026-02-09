using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Domain.UserSetCards.Commands;
using Lib.Domain.UserSetCards.Queries;
using Lib.Shared.DataModels.Entities.Itrs.UserSetCards;
using Lib.Shared.DataModels.Entities.Oufs.UserSetCards;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Domain.UserSetCards.Apis;

public sealed class UserSetCardsDomainService : IUserSetCardsDomainService
{
    private readonly IUserSetCardsQueryDomainService _queryService;
    private readonly IUserSetCardsCommandDomainService _commandService;

    public UserSetCardsDomainService(ILogger logger) : this(new UserSetCardsQueryDomainService(logger), new UserSetCardsCommandDomainService(logger))
    { }

    private UserSetCardsDomainService(IUserSetCardsQueryDomainService queryService, IUserSetCardsCommandDomainService commandService)
    {
        _queryService = queryService;
        _commandService = commandService;
    }

    public async Task<IOperationResponse<IUserSetCardOufEntity>> UserSetCardByUserAndSetAsync(
        IUserSetCardItrEntity userSetCard,
        CancellationToken cancellationToken) => await _queryService.UserSetCardByUserAndSetAsync(userSetCard, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<IUserSetCardOufEntity>>> AllUserSetCardsAsync(
        IAllUserSetCardsItrEntity userSetCards,
        CancellationToken cancellationToken) => await _queryService.AllUserSetCardsAsync(userSetCards, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<IUserSetCardOufEntity>> AddSetGroupToUserSetCardAsync(
        IAddSetGroupToUserSetCardItrEntity entity,
        CancellationToken cancellationToken) => await _commandService.AddSetGroupToUserSetCardAsync(entity, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<IUserSetCardOufEntity>> AddCardToSetAsync(
        IAddCardToSetItrEntity entity,
        CancellationToken cancellationToken) => await _commandService.AddCardToSetAsync(entity, cancellationToken).ConfigureAwait(false);
}
