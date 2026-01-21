using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.UserCards.Apis;
using Lib.Adapter.UserCards.Apis.Entities;
using Lib.Aggregator.UserCards.Queries.Mappers;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.UserCards.Queries.UserCard;

internal sealed class UserCardAggregatorService : IUserCardAggregatorService
{
    private readonly IUserCardsAdapterService _userCardsAdapterService;
    private readonly ICollectionUserCardExtToItrMapper _collectionMapper;
    private readonly IUserCardItrToXfrMapper _userCardItrToXfrMapper;

    public UserCardAggregatorService(ILogger logger) : this(
        new UserCardsAdapterService(logger),
        new CollectionUserCardExtToItrMapper(),
        new UserCardItrToXfrMapper())
    { }

    private UserCardAggregatorService(
        IUserCardsAdapterService userCardsAdapterService,
        ICollectionUserCardExtToItrMapper collectionMapper,
        IUserCardItrToXfrMapper userCardItrToXfrMapper)
    {
        _userCardsAdapterService = userCardsAdapterService;
        _collectionMapper = collectionMapper;
        _userCardItrToXfrMapper = userCardItrToXfrMapper;
    }

    public async Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> Execute(IUserCardItrEntity input)
    {
        IUserCardXfrEntity xfrEntity = await _userCardItrToXfrMapper.Map(input).ConfigureAwait(false);
        IOperationResponse<IEnumerable<UserCardExtEntity>> response = await _userCardsAdapterService.UserCardAsync(xfrEntity).ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<IEnumerable<IUserCardOufEntity>>(response.OuterException);
        }

        IEnumerable<IUserCardOufEntity> mappedUserCards = await _collectionMapper.Map(response.ResponseData).ConfigureAwait(false);
        return new SuccessOperationResponse<IEnumerable<IUserCardOufEntity>>(mappedUserCards);
    }

}
