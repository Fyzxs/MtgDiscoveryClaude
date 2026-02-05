using System.Threading;
using System.Threading.Tasks;
using Lib.Aggregator.UserCards.Apis;
using Lib.Shared.DataModels.Entities.Itrs.UserCards;
using Lib.Shared.DataModels.Entities.Oufs.UserCards.Signing;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Domain.UserCards.Queries;

/// <summary>
/// Single-method service for retrieving user cards by multiple artists for convention signing planning.
/// Delegates to aggregator layer for data retrieval and grouping.
/// </summary>
internal sealed class UserCardsForSigningDomainService : IUserCardsForSigningDomainService
{
    private readonly IUserCardsQueryAggregatorService _userCardsAggregatorService;

    public UserCardsForSigningDomainService(ILogger logger) : this(new UserCardsAggregatorService(logger))
    { }

    private UserCardsForSigningDomainService(IUserCardsQueryAggregatorService userCardsAggregatorService) => _userCardsAggregatorService = userCardsAggregatorService;

    public async Task<IOperationResponse<ISigningResultOufEntity>> Execute(
        IUserCardsForSigningItrEntity input,
        CancellationToken cancellationToken) => await _userCardsAggregatorService.UserCardsForSigningAsync(input, cancellationToken).ConfigureAwait(false);
}
