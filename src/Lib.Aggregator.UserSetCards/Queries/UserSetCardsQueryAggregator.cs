using System.Threading.Tasks;
using Lib.Aggregator.UserSetCards.Apis;
using Lib.Aggregator.UserSetCards.Queries.GetUserSetCard;
using Lib.Shared.DataModels.Entities.Itrs.UserSetCards;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.UserSetCards.Queries;

internal sealed class UserSetCardsQueryAggregator : IUserSetCardsQueryAggregator
{
    private readonly IGetUserSetCardAggregatorService _getUserSetCardOperations;

    public UserSetCardsQueryAggregator(ILogger logger) : this(new GetUserSetCardAggregatorService(logger))
    { }

    private UserSetCardsQueryAggregator(IGetUserSetCardAggregatorService getUserSetCardOperations) => _getUserSetCardOperations = getUserSetCardOperations;

    public Task<IOperationResponse<IUserSetCardOufEntity>> GetUserSetCardByUserAndSetAsync(IUserSetCardItrEntity userSetCard) => _getUserSetCardOperations.Execute(userSetCard);
}
