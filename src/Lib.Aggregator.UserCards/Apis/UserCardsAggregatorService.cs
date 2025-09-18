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
    private readonly IUserCardItrToXfrMapper _userCardItrToXfrMapper;
    private readonly IUserCardsSetItrToXfrMapper _userCardsSetItrToXfrMapper;

    public UserCardsAggregatorService(ILogger logger) : this(
        new UserCardsAdapterService(logger),
        new UserCardExtToItrEntityMapper(),
        new CollectionUserCardExtToItrMapper(),
        new UserCardItrToXfrMapper(),
        new UserCardsSetItrToXfrMapper())
    { }

    private UserCardsAggregatorService(
        IUserCardsAdapterService userCardsAdapterService,
        IUserCardExtToItrEntityMapper userCardMapper,
        ICollectionUserCardExtToItrMapper collectionMapper,
        IUserCardItrToXfrMapper userCardItrToXfrMapper,
        IUserCardsSetItrToXfrMapper userCardsSetItrToXfrMapper)
    {
        _userCardsAdapterService = userCardsAdapterService;
        _userCardMapper = userCardMapper;
        _collectionMapper = collectionMapper;
        _userCardItrToXfrMapper = userCardItrToXfrMapper;
        _userCardsSetItrToXfrMapper = userCardsSetItrToXfrMapper;
    }

    public async Task<IOperationResponse<IUserCardItrEntity>> AddUserCardAsync(IUserCardItrEntity userCard)
    {
        IUserCardXfrEntity xfrEntity = await _userCardItrToXfrMapper.Map(userCard).ConfigureAwait(false);
        IOperationResponse<UserCardExtEntity> response = await _userCardsAdapterService.AddUserCardAsync(xfrEntity).ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<IUserCardItrEntity>(response.OuterException);
        }

        IUserCardItrEntity mappedUserCard = await _userCardMapper.Map(response.ResponseData).ConfigureAwait(false);
        return new SuccessOperationResponse<IUserCardItrEntity>(mappedUserCard);
    }

    public async Task<IOperationResponse<IEnumerable<IUserCardItrEntity>>> UserCardsBySetAsync(IUserCardsSetItrEntity userCardsSet)
    {
        IUserCardsSetXfrEntity xfrEntity = await _userCardsSetItrToXfrMapper.Map(userCardsSet).ConfigureAwait(false);
        IOperationResponse<IEnumerable<UserCardExtEntity>> response = await _userCardsAdapterService.UserCardsBySetAsync(xfrEntity).ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<IEnumerable<IUserCardItrEntity>>(response.OuterException);
        }

        IEnumerable<IUserCardItrEntity> mappedUserCards = await _collectionMapper.Map(response.ResponseData).ConfigureAwait(false);
        return new SuccessOperationResponse<IEnumerable<IUserCardItrEntity>>(mappedUserCards);
    }
}
