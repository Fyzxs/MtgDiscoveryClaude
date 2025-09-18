using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.UserCards.Apis;
using Lib.Adapter.UserCards.Apis.Entities;
using Lib.Aggregator.UserCards.Commands.Mappers;
using Lib.Aggregator.UserCards.Queries.Mappers;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.UserCards.Apis;

public sealed class UserCardsAggregatorService : IUserCardsAggregatorService
{
    private readonly IUserCardsAdapterService _userCardsAdapterService;
    private readonly IUserCardExtToItrEntityMapper _userCardMapper;
    private readonly ICollectionUserCardExtToItrMapper _collectionMapper;
    private readonly IAddUserCardItrToXfrMapper _addUserCardItrToXfrMapper;
    private readonly IUserCardsSetItrToXfrMapper _userCardsSetItrToXfrMapper;

    public UserCardsAggregatorService(ILogger logger) : this(
        new UserCardsAdapterService(logger),
        new UserCardExtToItrEntityMapper(),
        new CollectionUserCardExtToItrMapper(),
        new AddUserCardItrToXfrMapper(),
        new UserCardsSetItrToXfrMapper())
    { }

    private UserCardsAggregatorService(
        IUserCardsAdapterService userCardsAdapterService,
        IUserCardExtToItrEntityMapper userCardMapper,
        ICollectionUserCardExtToItrMapper collectionMapper,
        IAddUserCardItrToXfrMapper addUserCardItrToXfrMapper,
        IUserCardsSetItrToXfrMapper userCardsSetItrToXfrMapper)
    {
        _userCardsAdapterService = userCardsAdapterService;
        _userCardMapper = userCardMapper;
        _collectionMapper = collectionMapper;
        _addUserCardItrToXfrMapper = addUserCardItrToXfrMapper;
        _userCardsSetItrToXfrMapper = userCardsSetItrToXfrMapper;
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

    public async Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardsBySetAsync(IUserCardsSetItrEntity userCardsSet)
    {
        IUserCardsSetXfrEntity xfrEntity = await _userCardsSetItrToXfrMapper.Map(userCardsSet).ConfigureAwait(false);
        IOperationResponse<IEnumerable<UserCardExtEntity>> response = await _userCardsAdapterService.UserCardsBySetAsync(xfrEntity).ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<IEnumerable<IUserCardOufEntity>>(response.OuterException);
        }

        IEnumerable<IUserCardOufEntity> mappedUserCards = await _collectionMapper.Map(response.ResponseData).ConfigureAwait(false);
        return new SuccessOperationResponse<IEnumerable<IUserCardOufEntity>>(mappedUserCards);
    }
}
