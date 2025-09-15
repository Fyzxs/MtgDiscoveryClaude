using System.Threading.Tasks;
using Lib.Aggregator.UserCards.Apis;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Exceptions;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Domain.UserCards.Apis;

public sealed class UserCardsDomainService : IUserCardsDomainService
{
    private readonly IUserCardsAggregatorService _userCardsAggregatorService;

    public UserCardsDomainService(ILogger logger) : this(new UserCardsAggregatorService(logger))
    { }

    private UserCardsDomainService(IUserCardsAggregatorService userCardsAggregatorService) => _userCardsAggregatorService = userCardsAggregatorService;

    public async Task<IOperationResponse<IUserCardCollectionItrEntity>> AddUserCardAsync(IUserCardCollectionItrEntity userCard)
    {
        return await _userCardsAggregatorService.AddUserCardAsync(userCard).ConfigureAwait(false);
    }
}
