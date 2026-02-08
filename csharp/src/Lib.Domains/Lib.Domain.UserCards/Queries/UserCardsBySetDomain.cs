using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Aggregator.UserCards.Apis;
using Lib.Shared.DataModels.Entities.Itrs.UserCards;
using Lib.Shared.DataModels.Entities.Oufs.UserCards;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Domain.UserCards.Queries;

/// <summary>
/// Single-method service for retrieving all user cards for a specific user within a given set.
/// Delegates to aggregator layer for data retrieval.
/// </summary>
internal sealed class UserCardsBySetDomain : IUserCardsBySetDomain
{
    private readonly IUserCardsQueryAggregatorService _userCardsAggregatorService;

    public UserCardsBySetDomain(ILogger logger) : this(new UserCardsAggregatorService(logger))
    { }

    private UserCardsBySetDomain(IUserCardsQueryAggregatorService userCardsAggregatorService) => _userCardsAggregatorService = userCardsAggregatorService;

    public async Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> Execute(
        IUserCardsSetItrEntity input,
        CancellationToken cancellationToken) => await _userCardsAggregatorService.UserCardsBySetAsync(input, cancellationToken).ConfigureAwait(false);
}
