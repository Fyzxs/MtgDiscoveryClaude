using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Aggregator.UserCards.Apis;
using Lib.Aggregator.UserCards.Commands.AddUserCard;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.UserCards.Commands;

internal sealed class UserCardsCommandAggregator : IUserCardsCommandAggregatorService
{
    private readonly IAddUserCardAggregatorService _addUserCardOperations;

    public UserCardsCommandAggregator(ILogger logger) : this(new AddUserCardAggregatorService(logger)) { }

    private UserCardsCommandAggregator(IAddUserCardAggregatorService addUserCardOperations) => _addUserCardOperations = addUserCardOperations;

    public Task<IOperationResponse<IUserCardOufEntity>> AddUserCardAsync(IUserCardItrEntity userCard) => _addUserCardOperations.Execute(userCard);
}
