using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.UserCards.Apis;
using Lib.Adapter.UserCards.Apis.Entities;
using Lib.Aggregator.UserCards.Queries.Mappers;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.UserCards.Queries.UserCardsByName;

internal sealed class UserCardsByNameAggregatorService : IUserCardsByNameAggregatorService
{
    private readonly IUserCardsAdapterService _userCardsAdapterService;
    private readonly ICollectionUserCardExtToItrMapper _collectionMapper;
    private readonly IUserCardsNameItrToXfrMapper _userCardsNameItrToXfrMapper;

    public UserCardsByNameAggregatorService(ILogger logger) : this(
        new UserCardsAdapterService(logger),
        new CollectionUserCardExtToItrMapper(),
        new UserCardsNameItrToXfrMapper())
    { }

    private UserCardsByNameAggregatorService(
        IUserCardsAdapterService userCardsAdapterService,
        ICollectionUserCardExtToItrMapper collectionMapper,
        IUserCardsNameItrToXfrMapper userCardsNameItrToXfrMapper)
    {
        _userCardsAdapterService = userCardsAdapterService;
        _collectionMapper = collectionMapper;
        _userCardsNameItrToXfrMapper = userCardsNameItrToXfrMapper;
    }

    public async Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardsByNameAsync(IUserCardsNameItrEntity userCardsName)
    {
        IUserCardsNameXfrEntity xfrEntity = await _userCardsNameItrToXfrMapper.Map(userCardsName).ConfigureAwait(false);
        IOperationResponse<IEnumerable<UserCardExtEntity>> response = await _userCardsAdapterService.UserCardsByNameAsync(xfrEntity).ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<IEnumerable<IUserCardOufEntity>>(response.OuterException);
        }

        IEnumerable<IUserCardOufEntity> mappedUserCards = await _collectionMapper.Map(response.ResponseData).ConfigureAwait(false);
        return new SuccessOperationResponse<IEnumerable<IUserCardOufEntity>>(mappedUserCards);
    }

}
