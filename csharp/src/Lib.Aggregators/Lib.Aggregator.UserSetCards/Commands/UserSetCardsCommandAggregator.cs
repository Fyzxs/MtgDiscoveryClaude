using System.Threading.Tasks;
using Lib.Aggregator.UserSetCards.Apis;
using Lib.Shared.DataModels.Entities.Itrs.UserSetCards;
using Lib.Shared.DataModels.Entities.Oufs.UserSetCards;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.UserSetCards.Commands;

internal sealed class UserSetCardsCommandAggregator : IUserSetCardsCommandAggregator
{
    private readonly IAddSetGroupAggregatorService _addSetGroupOperations;
    private readonly IAddCardToSetAggregatorService _addCardToSetOperations;

    public UserSetCardsCommandAggregator(ILogger logger) : this(
        new AddSetGroupAggregatorService(logger),
        new AddCardToSetAggregatorService(logger))
    { }

    private UserSetCardsCommandAggregator(
        IAddSetGroupAggregatorService addSetGroupOperations,
        IAddCardToSetAggregatorService addCardToSetOperations)
    {
        _addSetGroupOperations = addSetGroupOperations;
        _addCardToSetOperations = addCardToSetOperations;
    }

    public Task<IOperationResponse<IUserSetCardOufEntity>> AddSetGroupToUserSetCardAsync(IAddSetGroupToUserSetCardItrEntity entity) => _addSetGroupOperations.Execute(entity);

    public Task<IOperationResponse<IUserSetCardOufEntity>> AddCardToSetAsync(IAddCardToSetItrEntity entity) => _addCardToSetOperations.Execute(entity);
}
