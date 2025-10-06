using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.UserCards.Apis;
using Lib.Adapter.UserCards.Apis.Entities;
using Lib.Aggregator.UserCards.Apis;
using Lib.Aggregator.UserCards.Queries.Mappers;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.UserCards.Queries;

internal sealed class UserCardsQueryAggregator : IUserCardsAggregatorService
{
    private readonly IUserCardsAdapterService _userCardsAdapterService;
    private readonly ICollectionUserCardExtToItrMapper _collectionMapper;
    private readonly IUserCardsSetItrToXfrMapper _userCardsSetItrToXfrMapper;
    private readonly IUserCardItrToXfrMapper _userCardItrToXfrMapper;
    private readonly IUserCardsByIdsItrToXfrMapper _userCardsByIdsItrToXfrMapper;

    public UserCardsQueryAggregator(ILogger logger) : this(
        new UserCardsAdapterService(logger),
        new CollectionUserCardExtToItrMapper(),
        new UserCardsSetItrToXfrMapper(),
        new UserCardItrToXfrMapper(),
        new UserCardsByIdsItrToXfrMapper())
    { }

    private UserCardsQueryAggregator(
        IUserCardsAdapterService userCardsAdapterService,
        ICollectionUserCardExtToItrMapper collectionMapper,
        IUserCardsSetItrToXfrMapper userCardsSetItrToXfrMapper,
        IUserCardItrToXfrMapper userCardItrToXfrMapper,
        IUserCardsByIdsItrToXfrMapper userCardsByIdsItrToXfrMapper)
    {
        _userCardsAdapterService = userCardsAdapterService;
        _collectionMapper = collectionMapper;
        _userCardsSetItrToXfrMapper = userCardsSetItrToXfrMapper;
        _userCardItrToXfrMapper = userCardItrToXfrMapper;
        _userCardsByIdsItrToXfrMapper = userCardsByIdsItrToXfrMapper;
    }

    public Task<IOperationResponse<IUserCardOufEntity>> AddUserCardAsync(IUserCardItrEntity userCard) => throw new System.NotImplementedException();

    public async Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardAsync(IUserCardItrEntity userCard)
    {
        IUserCardXfrEntity xfrEntity = await _userCardItrToXfrMapper.Map(userCard).ConfigureAwait(false);
        IOperationResponse<IEnumerable<UserCardExtEntity>> response = await _userCardsAdapterService.UserCardAsync(xfrEntity).ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<IEnumerable<IUserCardOufEntity>>(response.OuterException);
        }

        IEnumerable<IUserCardOufEntity> mappedUserCards = await _collectionMapper.Map(response.ResponseData).ConfigureAwait(false);
        return new SuccessOperationResponse<IEnumerable<IUserCardOufEntity>>(mappedUserCards);
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

    public async Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardsByIdsAsync(IUserCardsByIdsItrEntity userCards)
    {
        IUserCardsByIdsXfrEntity xfrEntity = await _userCardsByIdsItrToXfrMapper.Map(userCards).ConfigureAwait(false);
        IOperationResponse<IEnumerable<UserCardExtEntity>> response = await _userCardsAdapterService.UserCardsByIdsAsync(xfrEntity).ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<IEnumerable<IUserCardOufEntity>>(response.OuterException);
        }

        IEnumerable<IUserCardOufEntity> mappedUserCards = await _collectionMapper.Map(response.ResponseData).ConfigureAwait(false);
        return new SuccessOperationResponse<IEnumerable<IUserCardOufEntity>>(mappedUserCards);
    }
}
