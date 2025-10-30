using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Aggregator.UserCards.Apis;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Domain.UserCards.Queries;

/// <summary>
/// Single-method service for retrieving multiple user cards using batch point read operations.
/// Delegates to aggregator layer for data retrieval.
/// </summary>
internal sealed class UserCardsByIdsDomainService : IUserCardsByIdsDomainService
{
    private readonly IUserCardsQueryAggregatorService _userCardsAggregatorService;

    public UserCardsByIdsDomainService(ILogger logger) : this(new UserCardsAggregatorService(logger))
    { }

    private UserCardsByIdsDomainService(IUserCardsQueryAggregatorService userCardsAggregatorService) => _userCardsAggregatorService = userCardsAggregatorService;

    public async Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> Execute(IUserCardsByIdsItrEntity input) => await _userCardsAggregatorService.UserCardsByIdsAsync(input).ConfigureAwait(false);
}
