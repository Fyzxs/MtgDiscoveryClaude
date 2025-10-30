using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Aggregator.UserCards.Apis;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Domain.UserCards.Queries;

/// <summary>
/// Single-method service for retrieving a specific user card using point read operation.
/// Delegates to aggregator layer for data retrieval.
/// </summary>
internal sealed class UserCardDomainService : IUserCardDomainService
{
    private readonly IUserCardsQueryAggregatorService _userCardsAggregatorService;

    public UserCardDomainService(ILogger logger) : this(new UserCardsAggregatorService(logger))
    { }

    private UserCardDomainService(IUserCardsQueryAggregatorService userCardsAggregatorService) => _userCardsAggregatorService = userCardsAggregatorService;

    public async Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> Execute(IUserCardItrEntity input) => await _userCardsAggregatorService.UserCardAsync(input).ConfigureAwait(false);
}
