using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.UserCards.Apis;
using Lib.Adapter.UserCards.Apis.Entities;
using Lib.Aggregator.UserCards.Queries.Mappers;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.UserCards.Queries.UserCardsByIds;

internal sealed class UserCardsByIdsAggregatorService : IUserCardsByIdsAggregatorService
{
    private readonly IUserCardsAdapterService _userCardsAdapterService;
    private readonly ICollectionUserCardExtToItrMapper _collectionMapper;
    private readonly IUserCardsByIdsItrToXfrMapper _userCardsByIdsItrToXfrMapper;

    public UserCardsByIdsAggregatorService(ILogger logger) : this(
        new UserCardsAdapterService(logger),
        new CollectionUserCardExtToItrMapper(),
        new UserCardsByIdsItrToXfrMapper())
    { }

    private UserCardsByIdsAggregatorService(
        IUserCardsAdapterService userCardsAdapterService,
        ICollectionUserCardExtToItrMapper collectionMapper,
        IUserCardsByIdsItrToXfrMapper userCardsByIdsItrToXfrMapper)
    {
        _userCardsAdapterService = userCardsAdapterService;
        _collectionMapper = collectionMapper;
        _userCardsByIdsItrToXfrMapper = userCardsByIdsItrToXfrMapper;
    }

    public async Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> Execute(IUserCardsByIdsItrEntity input)
    {
        IUserCardsByIdsXfrEntity xfrEntity = await _userCardsByIdsItrToXfrMapper.Map(input).ConfigureAwait(false);
        IOperationResponse<IEnumerable<UserCardExtEntity>> response = await _userCardsAdapterService.UserCardsByIdsAsync(xfrEntity).ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<IEnumerable<IUserCardOufEntity>>(response.OuterException);
        }

        IEnumerable<IUserCardOufEntity> mappedUserCards = await _collectionMapper.Map(response.ResponseData).ConfigureAwait(false);
        return new SuccessOperationResponse<IEnumerable<IUserCardOufEntity>>(mappedUserCards);
    }

}
