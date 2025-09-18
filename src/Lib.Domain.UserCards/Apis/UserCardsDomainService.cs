using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Aggregator.UserCards.Apis;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Domain.UserCards.Apis;

public sealed class UserCardsDomainService : IUserCardsDomainService
{
    private readonly IUserCardsAggregatorService _userCardsAggregatorService;

    public UserCardsDomainService(ILogger logger) : this(new UserCardsAggregatorService(logger))
    { }

    private UserCardsDomainService(IUserCardsAggregatorService userCardsAggregatorService) => _userCardsAggregatorService = userCardsAggregatorService;

    public async Task<IOperationResponse<IUserCardItrEntity>> AddUserCardAsync(IUserCardItrEntity userCard)
    {
        return await _userCardsAggregatorService.AddUserCardAsync(userCard).ConfigureAwait(false);
    }

    public async Task<IOperationResponse<IEnumerable<IUserCardItrEntity>>> UserCardsBySetAsync(IUserCardsSetItrEntity userCardsSet)
    {
        return await _userCardsAggregatorService.UserCardsBySetAsync(userCardsSet).ConfigureAwait(false);
    }
}
