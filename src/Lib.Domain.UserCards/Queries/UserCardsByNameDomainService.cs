using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Aggregator.UserCards.Apis;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Domain.UserCards.Queries;

/// <summary>
/// Single-method service for retrieving all user cards with a specific card name.
/// Delegates to aggregator layer for data retrieval.
/// </summary>
internal sealed class UserCardsByNameDomainService : IUserCardsByNameDomainService
{
    private readonly IUserCardsQueryAggregatorService _userCardsAggregatorService;

    public UserCardsByNameDomainService(ILogger logger) : this(new UserCardsAggregatorService(logger))
    { }

    private UserCardsByNameDomainService(IUserCardsQueryAggregatorService userCardsAggregatorService) => _userCardsAggregatorService = userCardsAggregatorService;

    public async Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> Execute(IUserCardsNameItrEntity input) => await _userCardsAggregatorService.UserCardsByNameAsync(input).ConfigureAwait(false);
}
