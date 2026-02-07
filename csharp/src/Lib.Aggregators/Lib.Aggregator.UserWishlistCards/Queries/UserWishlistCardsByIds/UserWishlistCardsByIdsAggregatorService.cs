using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserWishlistCards;
using Lib.Adapter.UserWishlistCards.Apis;
using Lib.Adapter.UserWishlistCards.Apis.Entities;
using Lib.Aggregator.UserWishlistCards.Queries.Mappers;
using Lib.Shared.DataModels.Entities.Itrs.UserWishlistCards;
using Lib.Shared.DataModels.Entities.Oufs.UserWishlistCards;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.UserWishlistCards.Queries.UserWishlistCardsByIds;

internal sealed class UserWishlistCardsByIdsAggregatorService : IUserWishlistCardsByIdsAggregatorService
{
    private readonly IUserWishlistCardsAdapterService _userWishlistCardsAdapterService;
    private readonly ICollectionUserWishlistCardExtToOufMapper _collectionMapper;
    private readonly IUserWishlistCardsByIdsItrToXfrMapper _userWishlistCardsByIdsItrToXfrMapper;

    public UserWishlistCardsByIdsAggregatorService(ILogger logger) : this(
        new UserWishlistCardsAdapterService(logger),
        new CollectionUserWishlistCardExtToOufMapper(),
        new UserWishlistCardsByIdsItrToXfrMapper())
    { }

    private UserWishlistCardsByIdsAggregatorService(
        IUserWishlistCardsAdapterService userWishlistCardsAdapterService,
        ICollectionUserWishlistCardExtToOufMapper collectionMapper,
        IUserWishlistCardsByIdsItrToXfrMapper userWishlistCardsByIdsItrToXfrMapper)
    {
        _userWishlistCardsAdapterService = userWishlistCardsAdapterService;
        _collectionMapper = collectionMapper;
        _userWishlistCardsByIdsItrToXfrMapper = userWishlistCardsByIdsItrToXfrMapper;
    }

    public async Task<IOperationResponse<IEnumerable<IUserWishlistCardOufEntity>>> Execute(IUserWishlistCardsByIdsItrEntity input, CancellationToken cancellationToken)
    {
        IUserWishlistCardsByIdsXfrEntity xfrEntity = await _userWishlistCardsByIdsItrToXfrMapper.Map(input).ConfigureAwait(false);
        IOperationResponse<IEnumerable<UserWishlistCardExtEntity>> response = await _userWishlistCardsAdapterService.UserWishlistCardsByIdsAsync(xfrEntity, cancellationToken).ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<IEnumerable<IUserWishlistCardOufEntity>>(response.OuterException);
        }

        IEnumerable<IUserWishlistCardOufEntity> mappedUserWishlistCards = await _collectionMapper.Map(response.ResponseData).ConfigureAwait(false);
        return new SuccessOperationResponse<IEnumerable<IUserWishlistCardOufEntity>>(mappedUserWishlistCards);
    }
}
