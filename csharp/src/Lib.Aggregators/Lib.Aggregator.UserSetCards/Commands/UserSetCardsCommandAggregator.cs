using System.Threading;
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

    public async Task<IOperationResponse<IUserSetCardOufEntity>> AddSetGroupToUserSetCardAsync(
        IAddSetGroupToUserSetCardItrEntity entity,
        CancellationToken cancellationToken) => await _addSetGroupOperations.Execute(entity, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<IUserSetCardOufEntity>> AddCardToSetAsync(
        IAddCardToSetItrEntity entity,
        CancellationToken cancellationToken) => await _addCardToSetOperations.Execute(entity, cancellationToken).ConfigureAwait(false);
}
