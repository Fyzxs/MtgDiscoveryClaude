using System.Threading.Tasks;
using Lib.Aggregator.UserSetCards.Commands;
using Lib.Aggregator.UserSetCards.Entities;
using Lib.Aggregator.UserSetCards.Queries;
using Lib.Shared.DataModels.Entities.Itrs.UserSetCards;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.UserSetCards.Apis;

public sealed class UserSetCardsAggregatorService : IUserSetCardsAggregatorService
{
    private readonly IUserSetCardsQueryAggregator _queryOperations;
    private readonly IUserSetCardsCommandAggregator _commandOperations;

    public UserSetCardsAggregatorService(ILogger logger) : this(
        new UserSetCardsQueryAggregator(logger),
        new UserSetCardsCommandAggregator(logger))
    { }

    private UserSetCardsAggregatorService(
        IUserSetCardsQueryAggregator queryOperations,
        IUserSetCardsCommandAggregator commandOperations)
    {
        _queryOperations = queryOperations;
        _commandOperations = commandOperations;
    }

    public Task<IOperationResponse<IUserSetCardOufEntity>> GetUserSetCardByUserAndSetAsync(IUserSetCardItrEntity userSetCard) => _queryOperations.GetUserSetCardByUserAndSetAsync(userSetCard);

    public Task<IOperationResponse<IUserSetCardOufEntity>> AddSetGroupToUserSetCardAsync(IAddSetGroupToUserSetCardItrEntity entity) => _commandOperations.AddSetGroupToUserSetCardAsync(entity);
}
