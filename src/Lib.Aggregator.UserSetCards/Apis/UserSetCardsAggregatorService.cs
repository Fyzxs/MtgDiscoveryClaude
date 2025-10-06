using System.Threading.Tasks;
using Lib.Aggregator.UserSetCards.Entities;
using Lib.Aggregator.UserSetCards.Queries;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.UserSetCards.Apis;

public sealed class UserSetCardsAggregatorService : IUserSetCardsAggregatorService
{
    private readonly IUserSetCardsAggregatorService _queryOperations;

    public UserSetCardsAggregatorService(ILogger logger) : this(new UserSetCardsQueryAggregator(logger))
    { }

    private UserSetCardsAggregatorService(IUserSetCardsAggregatorService queryOperations) => _queryOperations = queryOperations;

    public Task<IOperationResponse<IUserSetCardOufEntity>> GetUserSetCardByUserAndSetAsync(IUserSetCardItrEntity userSetCard) => _queryOperations.GetUserSetCardByUserAndSetAsync(userSetCard);
}
