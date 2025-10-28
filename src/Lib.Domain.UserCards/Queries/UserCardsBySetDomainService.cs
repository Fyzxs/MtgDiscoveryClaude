using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Aggregator.UserCards.Apis;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;

namespace Lib.Domain.UserCards.Queries;

/// <summary>
/// Single-method service for retrieving all user cards for a specific user within a given set.
/// Delegates to aggregator layer for data retrieval.
/// </summary>
internal sealed class UserCardsBySetDomainService : IUserCardsBySetDomainService
{
    private readonly IUserCardsQueryAggregatorService _userCardsAggregatorService;

    public UserCardsBySetDomainService(IUserCardsQueryAggregatorService userCardsAggregatorService) => _userCardsAggregatorService = userCardsAggregatorService;

    public async Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> Execute(IUserCardsSetItrEntity input) => await _userCardsAggregatorService.UserCardsBySetAsync(input).ConfigureAwait(false);
}
