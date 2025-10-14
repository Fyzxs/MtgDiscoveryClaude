using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Aggregator.UserCards.Apis;
using Lib.Aggregator.UserCards.Commands.AddUserCard;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.UserCards.Commands;

internal sealed class UserCardsCommandAggregator : IUserCardsAggregatorService
{
    private readonly IAddUserCardAggregatorService _addUserCardOperations;

    public UserCardsCommandAggregator(ILogger logger) : this(new AddUserCardAggregatorService(logger)) { }

    private UserCardsCommandAggregator(IAddUserCardAggregatorService addUserCardOperations) => _addUserCardOperations = addUserCardOperations;

    public Task<IOperationResponse<IUserCardOufEntity>> AddUserCardAsync(IUserCardItrEntity userCard) => _addUserCardOperations.AddUserCardAsync(userCard);

    public Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardAsync(IUserCardItrEntity userCard) => throw new System.NotImplementedException("Use UserCardsQueryAggregator");

    public Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardsBySetAsync(IUserCardsSetItrEntity userCardsSet) => throw new System.NotImplementedException("Use UserCardsQueryAggregator");

    public Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardsByIdsAsync(IUserCardsByIdsItrEntity userCards) => throw new System.NotImplementedException("Use UserCardsQueryAggregator");

    public Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardsByArtistAsync(IUserCardsArtistItrEntity userCardsArtist) => throw new System.NotImplementedException("Use UserCardsQueryAggregator");

    public Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardsByNameAsync(IUserCardsNameItrEntity userCardsName) => throw new System.NotImplementedException("Use UserCardsQueryAggregator");
}
