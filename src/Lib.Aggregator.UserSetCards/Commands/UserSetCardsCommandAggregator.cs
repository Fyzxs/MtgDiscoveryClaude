using System.Threading.Tasks;
using Lib.Aggregator.UserSetCards.Apis;
using Lib.Shared.DataModels.Entities.Itrs.UserSetCards;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.UserSetCards.Commands;

internal sealed class UserSetCardsCommandAggregator : IUserSetCardsCommandAggregator
{
    private readonly IAddSetGroupAggregatorService _addSetGroupOperations;

    public UserSetCardsCommandAggregator(ILogger logger) : this(new AddSetGroupAggregatorService(logger))
    { }

    private UserSetCardsCommandAggregator(IAddSetGroupAggregatorService addSetGroupOperations) => _addSetGroupOperations = addSetGroupOperations;

    public Task<IOperationResponse<IUserSetCardOufEntity>> AddSetGroupToUserSetCardAsync(IAddSetGroupToUserSetCardItrEntity entity) => _addSetGroupOperations.Execute(entity);
}
