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
    private readonly IUserSetCardsQueryDomainService _queryOperations;
    private readonly IUserSetCardsCommandDomainService _commandOperations;

    public UserSetCardsDomainService(ILogger logger) : this(new QueryUserSetCardsDomainService(logger), new CommandUserSetCardsDomainService(logger))
    { }

    private UserSetCardsDomainService(IUserSetCardsQueryDomainService queryOperations, IUserSetCardsCommandDomainService commandOperations)
    {
        _queryOperations = queryOperations;
        _commandOperations = commandOperations;
    }

    public async Task<IOperationResponse<IUserSetCardOufEntity>> UserSetCardByUserAndSetAsync(
        IUserSetCardItrEntity userSetCard,
        CancellationToken cancellationToken) => await _queryOperations.UserSetCardByUserAndSetAsync(userSetCard, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<IUserSetCardOufEntity>>> AllUserSetCardsAsync(
        IAllUserSetCardsItrEntity userSetCards,
        CancellationToken cancellationToken) => await _queryOperations.AllUserSetCardsAsync(userSetCards, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<IUserSetCardOufEntity>> AddSetGroupToUserSetCardAsync(
        IAddSetGroupToUserSetCardItrEntity entity,
        CancellationToken cancellationToken) => await _commandOperations.AddSetGroupToUserSetCardAsync(entity, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<IUserSetCardOufEntity>> AddCardToSetAsync(
        IAddCardToSetItrEntity entity,
        CancellationToken cancellationToken) => await _commandOperations.AddCardToSetAsync(entity, cancellationToken).ConfigureAwait(false);
}
