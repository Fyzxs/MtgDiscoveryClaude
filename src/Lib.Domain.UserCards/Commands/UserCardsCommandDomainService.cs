using System.Threading.Tasks;
using Lib.Aggregator.UserCards.Apis;
using Lib.Domain.UserCards.Apis;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Domain.UserCards.Commands;

/// <summary>
/// Command domain service for user card operations.
/// Delegates to single-method services following the Execute pattern.
/// </summary>
internal sealed class UserCardsCommandDomainService : IUserCardsCommandDomainService
{
    private readonly IAddUserCardDomainService _addUserCardService;

    public UserCardsCommandDomainService(ILogger logger) : this(new AddUserCardDomainService(logger))
    { }

    private UserCardsCommandDomainService(IAddUserCardDomainService addUserCardService) => _addUserCardService = addUserCardService;

    public async Task<IOperationResponse<IUserCardOufEntity>> AddUserCardAsync(IUserCardItrEntity userCard) => await _addUserCardService.Execute(userCard).ConfigureAwait(false);
}
