using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Adapter.UserCards.Apis;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.UserCards.Apis;

public sealed class UserCardsAggregatorService : IUserCardsAggregatorService
{
    private readonly IUserCardsAdapterService _userCardsAdapterService;

    public UserCardsAggregatorService(ILogger logger) : this(new UserCardsAdapterService(logger))
    { }

    private UserCardsAggregatorService(IUserCardsAdapterService userCardsAdapterService) => _userCardsAdapterService = userCardsAdapterService;

    public async Task<IOperationResponse<IUserCardCollectionItrEntity>> AddUserCardAsync(IUserCardCollectionItrEntity userCard)
    {
        return await _userCardsAdapterService.AddUserCardAsync(userCard).ConfigureAwait(false);
    }

    public async Task<IOperationResponse<IEnumerable<IUserCardCollectionItrEntity>>> UserCardsBySetAsync(IUserCardsSetItrEntity userCardsSet)
    {
        return await _userCardsAdapterService.UserCardsBySetAsync(userCardsSet).ConfigureAwait(false);
    }
}
