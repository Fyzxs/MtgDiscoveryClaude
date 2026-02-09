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
/// Single-method service for retrieving a specific user card using point read operation.
/// Delegates to aggregator layer for data retrieval.
/// </summary>
internal sealed class UserCardDomain : IUserCardDomain
{
    private readonly IUserCardsQueryAggregatorService _userCardsAggregatorService;

    public UserCardDomain(ILogger logger) : this(new UserCardsAggregatorService(logger))
    { }

    private UserCardDomain(IUserCardsQueryAggregatorService userCardsAggregatorService) => _userCardsAggregatorService = userCardsAggregatorService;

    public async Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> Execute(
        IUserCardItrEntity input,
        CancellationToken cancellationToken) => await _userCardsAggregatorService.UserCardAsync(input, cancellationToken).ConfigureAwait(false);
}
