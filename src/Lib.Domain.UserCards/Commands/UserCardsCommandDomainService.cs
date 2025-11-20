using System.Threading.Tasks;
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
    private readonly IAddUserCardOnlyDomainService _addUserCardOnlyService;

    public UserCardsCommandDomainService(ILogger logger) : this(
        new AddUserCardDomainService(logger),
        new AddUserCardOnlyDomainService(logger))
    { }

    private UserCardsCommandDomainService(
        IAddUserCardDomainService addUserCardService,
        IAddUserCardOnlyDomainService addUserCardOnlyService)
    {
        _addUserCardService = addUserCardService;
        _addUserCardOnlyService = addUserCardOnlyService;
    }

    public async Task<IOperationResponse<IUserCardOufEntity>> AddUserCardAsync(IUserCardItrEntity userCard) => await _addUserCardService.Execute(userCard).ConfigureAwait(false);

    public async Task<IOperationResponse<IUserCardOufEntity>> AddUserCardOnlyAsync(IUserCardItrEntity userCard) => await _addUserCardOnlyService.Execute(userCard).ConfigureAwait(false);
}
