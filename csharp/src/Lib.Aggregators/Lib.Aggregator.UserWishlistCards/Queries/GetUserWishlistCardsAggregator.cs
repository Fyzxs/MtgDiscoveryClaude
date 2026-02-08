using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.UserWishlistCards.Apis;
using Lib.Aggregator.UserWishlistCards.Queries.Entities;
using Lib.Aggregator.UserWishlistCards.Queries.Mappers;
using Lib.Shared.DataModels.Entities.Itrs.UserWishlistCards;
using Lib.Shared.DataModels.Entities.Oufs.UserWishlistCards;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.UserWishlistCards.Queries;

internal sealed class GetUserWishlistCardsAggregator : IGetUserWishlistCardsAggregator
{
    private readonly IUserWishlistCardsAdapterService _userWishlistCardsAdapterService;
    private readonly IGetUserWishlistCardsItrToXfrMapper _itrToXfrMapper;
    private readonly ICollectionUserWishlistCardExtToOufMapper _extToOufMapper;

    public GetUserWishlistCardsAggregator(ILogger logger) : this(
        new UserWishlistCardsAdapterService(logger),
        new GetUserWishlistCardsItrToXfrMapper(),
        new CollectionUserWishlistCardExtToOufMapper())
    { }

    private GetUserWishlistCardsAggregator(
        IUserWishlistCardsAdapterService userWishlistCardsAdapterService,
        IGetUserWishlistCardsItrToXfrMapper itrToXfrMapper,
        ICollectionUserWishlistCardExtToOufMapper extToOufMapper)
    {
        _userWishlistCardsAdapterService = userWishlistCardsAdapterService;
        _itrToXfrMapper = itrToXfrMapper;
        _extToOufMapper = extToOufMapper;
    }

    public async Task<IOperationResponse<IEnumerable<IUserWishlistCardOufEntity>>> Execute(IUserWishlistCardsQueryItrEntity input, CancellationToken cancellationToken)
    {
        UserWishlistCardsQueryXfrEntity xfrEntity = await _itrToXfrMapper.Map(input).ConfigureAwait(false);

        IOperationResponse<IEnumerable<Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserWishlistCards.UserWishlistCardExtEntity>> response = await _userWishlistCardsAdapterService.UserWishlistCardsByUserAsync(xfrEntity, cancellationToken).ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<IEnumerable<IUserWishlistCardOufEntity>>(response.OuterException);
        }

        IEnumerable<IUserWishlistCardOufEntity> mappedWishlistCards = await _extToOufMapper.Map(response.ResponseData).ConfigureAwait(false);
        return new SuccessOperationResponse<IEnumerable<IUserWishlistCardOufEntity>>(mappedWishlistCards);
    }
}
