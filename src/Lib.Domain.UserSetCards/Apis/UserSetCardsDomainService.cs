using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Domain.UserSetCards.Commands;
using Lib.Domain.UserSetCards.Queries;
using Lib.Shared.DataModels.Entities.Itrs.UserSetCards;
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

    public Task<IOperationResponse<IUserSetCardOufEntity>> UserSetCardByUserAndSetAsync(IUserSetCardItrEntity userSetCard) => _queryOperations.UserSetCardByUserAndSetAsync(userSetCard);

    public Task<IOperationResponse<IEnumerable<IUserSetCardOufEntity>>> AllUserSetCardsAsync(IAllUserSetCardsItrEntity userSetCards) =>
        _queryOperations.AllUserSetCardsAsync(userSetCards);

    public Task<IOperationResponse<IUserSetCardOufEntity>> AddSetGroupToUserSetCardAsync(IAddSetGroupToUserSetCardItrEntity entity) => _commandOperations.AddSetGroupToUserSetCardAsync(entity);
}
