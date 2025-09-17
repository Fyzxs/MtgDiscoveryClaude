using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.UserCards.Apis;
using Lib.Aggregator.UserCards.Commands.Mappers;
using Lib.Aggregator.UserCards.Queries.Mappers;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.UserCards.Apis;

public sealed class UserCardsAggregatorService : IUserCardsAggregatorService
{
    private readonly IUserCardsAdapterService _userCardsAdapterService;
    private readonly IUserCardExtToItrEntityMapper _userCardMapper;
    private readonly ICollectionUserCardExtToItrMapper _collectionMapper;

    public UserCardsAggregatorService(ILogger logger) : this(
        new UserCardsAdapterService(logger),
        new UserCardExtToItrEntityMapper(),
        new CollectionUserCardExtToItrMapper())
    { }

    private UserCardsAggregatorService(
        IUserCardsAdapterService userCardsAdapterService,
        IUserCardExtToItrEntityMapper userCardMapper,
        ICollectionUserCardExtToItrMapper collectionMapper)
    {
        _userCardsAdapterService = userCardsAdapterService;
        _userCardMapper = userCardMapper;
        _collectionMapper = collectionMapper;
    }

    public async Task<IOperationResponse<IUserCardItrEntity>> AddUserCardAsync(IUserCardItrEntity userCard)
    {
        IOperationResponse<UserCardExtEntity> response = await _userCardsAdapterService.AddUserCardAsync(userCard).ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<IUserCardItrEntity>(response.OuterException);
        }

        IUserCardItrEntity mappedUserCard = await _userCardMapper.Map(response.ResponseData).ConfigureAwait(false);
        return new SuccessOperationResponse<IUserCardItrEntity>(mappedUserCard);
    }

    public async Task<IOperationResponse<IEnumerable<IUserCardItrEntity>>> UserCardsBySetAsync(IUserCardsSetItrEntity userCardsSet)
    {
        IOperationResponse<IEnumerable<UserCardExtEntity>> response = await _userCardsAdapterService.UserCardsBySetAsync(userCardsSet).ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<IEnumerable<IUserCardItrEntity>>(response.OuterException);
        }

        IEnumerable<IUserCardItrEntity> mappedUserCards = await _collectionMapper.Map(response.ResponseData).ConfigureAwait(false);
        return new SuccessOperationResponse<IEnumerable<IUserCardItrEntity>>(mappedUserCards);
    }
}
