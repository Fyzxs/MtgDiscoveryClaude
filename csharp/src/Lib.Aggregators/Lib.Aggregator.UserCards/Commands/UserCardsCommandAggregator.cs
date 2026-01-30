using System.Threading.Tasks;
using Lib.Aggregator.UserCards.Apis;
using Lib.Shared.DataModels.Entities.Itrs.UserCards;
using Lib.Shared.DataModels.Entities.Oufs.UserCards;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.UserCards.Commands;

internal sealed class UserCardsCommandAggregator : IUserCardsCommandAggregatorService
{
    private readonly IAddUserCardAggregatorService _addUserCardOperations;
    private readonly IAddUserCardOnlyAggregatorService _addUserCardOnlyOperations;

    public UserCardsCommandAggregator(ILogger logger) : this(
        new AddUserCardAggregatorService(logger),
        new AddUserCardOnlyAggregatorService(logger))
    { }

    private UserCardsCommandAggregator(
        IAddUserCardAggregatorService addUserCardOperations,
        IAddUserCardOnlyAggregatorService addUserCardOnlyOperations)
    {
        _addUserCardOperations = addUserCardOperations;
        _addUserCardOnlyOperations = addUserCardOnlyOperations;
    }

    public Task<IOperationResponse<IUserCardOufEntity>> AddUserCardAsync(IUserCardItrEntity userCard) => _addUserCardOperations.Execute(userCard);

    public Task<IOperationResponse<IUserCardOufEntity>> AddUserCardOnlyAsync(IUserCardItrEntity userCard) => _addUserCardOnlyOperations.Execute(userCard);
}
