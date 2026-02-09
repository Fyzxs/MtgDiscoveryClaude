using System.Threading;
using System.Threading.Tasks;
using Lib.Aggregator.UserCards.Apis;
using Lib.Shared.DataModels.Entities.Itrs.UserCards;
using Lib.Shared.DataModels.Entities.Oufs.UserCards;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Domain.UserCards.Commands;

/// <summary>
/// Single-method service for adding a user card without updating UserSetCards.
/// Delegates to aggregator layer for UserCards-only data persistence.
/// </summary>
internal sealed class AddUserCardOnlyDomain : IAddUserCardOnlyDomain
{
    private readonly IUserCardsCommandAggregatorService _userCardsAggregatorService;

    public AddUserCardOnlyDomain(ILogger logger) : this(new UserCardsAggregatorService(logger))
    { }

    private AddUserCardOnlyDomain(IUserCardsCommandAggregatorService userCardsAggregatorService) => _userCardsAggregatorService = userCardsAggregatorService;

    public async Task<IOperationResponse<IUserCardOufEntity>> Execute(
        IUserCardItrEntity input,
        CancellationToken cancellationToken) => await _userCardsAggregatorService.AddUserCardOnlyAsync(input, cancellationToken).ConfigureAwait(false);
}
