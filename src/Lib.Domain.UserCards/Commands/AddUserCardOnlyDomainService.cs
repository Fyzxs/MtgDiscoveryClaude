using System.Threading.Tasks;
using Lib.Aggregator.UserCards.Apis;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Domain.UserCards.Commands;

/// <summary>
/// Single-method service for adding a user card without updating UserSetCards.
/// Delegates to aggregator layer for UserCards-only data persistence.
/// </summary>
internal sealed class AddUserCardOnlyDomainService : IAddUserCardOnlyDomainService
{
    private readonly IUserCardsCommandAggregatorService _userCardsAggregatorService;

    public AddUserCardOnlyDomainService(ILogger logger) : this(new UserCardsAggregatorService(logger))
    { }

    private AddUserCardOnlyDomainService(IUserCardsCommandAggregatorService userCardsAggregatorService) => _userCardsAggregatorService = userCardsAggregatorService;

    public async Task<IOperationResponse<IUserCardOufEntity>> Execute(IUserCardItrEntity input) => await _userCardsAggregatorService.AddUserCardOnlyAsync(input).ConfigureAwait(false);
}
