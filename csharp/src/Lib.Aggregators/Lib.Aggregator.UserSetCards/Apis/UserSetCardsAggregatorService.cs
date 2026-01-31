using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Aggregator.UserSetCards.Commands;
using Lib.Aggregator.UserSetCards.Queries;
using Lib.Shared.DataModels.Entities.Itrs.UserSetCards;
using Lib.Shared.DataModels.Entities.Oufs.UserSetCards;
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
    {
    }

    private UserSetCardsAggregatorService(
        IUserSetCardsQueryAggregator queryOperations,
        IUserSetCardsCommandAggregator commandOperations)
    {
        _queryOperations = queryOperations;
        _commandOperations = commandOperations;
    }

    public async Task<IOperationResponse<IUserSetCardOufEntity>> UserSetCardByUserAndSetAsync(IUserSetCardItrEntity userSetCard) => await _queryOperations.UserSetCardByUserAndSetAsync(userSetCard);

    public async Task<IOperationResponse<IEnumerable<IUserSetCardOufEntity>>> AllUserSetCardsAsync(IAllUserSetCardsItrEntity userSetCards) => await _queryOperations.AllUserSetCardsAsync(userSetCards);

    public async Task<IOperationResponse<IUserSetCardOufEntity>> AddSetGroupToUserSetCardAsync(IAddSetGroupToUserSetCardItrEntity entity) => await _commandOperations.AddSetGroupToUserSetCardAsync(entity);

    public Task<IOperationResponse<IUserSetCardOufEntity>> AddCardToSetAsync(IAddCardToSetItrEntity entity) => throw new System.NotImplementedException();
}
