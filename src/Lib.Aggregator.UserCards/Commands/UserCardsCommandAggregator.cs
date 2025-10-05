using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.UserCards.Apis;
using Lib.Adapter.UserCards.Apis.Entities;
using Lib.Aggregator.UserCards.Apis;
using Lib.Aggregator.UserCards.Commands.Mappers;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.UserCards.Commands;

internal sealed class UserCardsCommandAggregator : IUserCardsAggregatorService
{
    private readonly IUserCardsAdapterService _userCardsAdapterService;
    private readonly IUserCardExtToItrEntityMapper _userCardMapper;
    private readonly IAddUserCardItrToXfrMapper _addUserCardItrToXfrMapper;

    public UserCardsCommandAggregator(ILogger logger) : this(
        new UserCardsAdapterService(logger),
        new UserCardExtToItrEntityMapper(),
        new AddUserCardItrToXfrMapper())
    { }

    private UserCardsCommandAggregator(
        IUserCardsAdapterService userCardsAdapterService,
        IUserCardExtToItrEntityMapper userCardMapper,
        IAddUserCardItrToXfrMapper addUserCardItrToXfrMapper)
    {
        _userCardsAdapterService = userCardsAdapterService;
        _userCardMapper = userCardMapper;
        _addUserCardItrToXfrMapper = addUserCardItrToXfrMapper;
    }

    public async Task<IOperationResponse<IUserCardOufEntity>> AddUserCardAsync(IUserCardItrEntity userCard)
    {
        IAddUserCardXfrEntity xfrEntity = await _addUserCardItrToXfrMapper.Map(userCard).ConfigureAwait(false);
        IOperationResponse<UserCardExtEntity> response = await _userCardsAdapterService.AddUserCardAsync(xfrEntity).ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<IUserCardOufEntity>(response.OuterException);
        }

        IUserCardOufEntity mappedUserCard = await _userCardMapper.Map(response.ResponseData).ConfigureAwait(false);
        return new SuccessOperationResponse<IUserCardOufEntity>(mappedUserCard);
    }

    public Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardAsync(IUserCardItrEntity userCard) => throw new System.NotImplementedException();

    public Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardsBySetAsync(IUserCardsSetItrEntity userCardsSet) => throw new System.NotImplementedException();

    public Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardsByIdsAsync(IUserCardsByIdsItrEntity userCards) => throw new System.NotImplementedException();
}
