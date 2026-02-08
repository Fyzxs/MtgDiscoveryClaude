using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserWishlistCards;
using Lib.Adapter.UserWishlistCards.Apis;
using Lib.Aggregator.UserWishlistCards.Commands.Entities;
using Lib.Aggregator.UserWishlistCards.Commands.Mappers;
using Lib.Shared.DataModels.Entities.Itrs.UserWishlistCards;
using Lib.Shared.DataModels.Entities.Oufs.UserWishlistCards;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.UserWishlistCards.Commands;

internal sealed class RemoveUserWishlistCardAggregator : IRemoveUserWishlistCardAggregator
{
    private readonly IUserWishlistCardsAdapterService _userWishlistCardsAdapterService;
    private readonly IRemoveUserWishlistCardItrToXfrMapper _itrToXfrMapper;
    private readonly IUserWishlistCardExtToOufEntityMapper _extToOufMapper;

    public RemoveUserWishlistCardAggregator(ILogger logger) : this(
        new UserWishlistCardsAdapterService(logger),
        new RemoveUserWishlistCardItrToXfrMapper(),
        new UserWishlistCardExtToOufEntityMapper())
    { }

    private RemoveUserWishlistCardAggregator(
        IUserWishlistCardsAdapterService userWishlistCardsAdapterService,
        IRemoveUserWishlistCardItrToXfrMapper itrToXfrMapper,
        IUserWishlistCardExtToOufEntityMapper extToOufMapper)
    {
        _userWishlistCardsAdapterService = userWishlistCardsAdapterService;
        _itrToXfrMapper = itrToXfrMapper;
        _extToOufMapper = extToOufMapper;
    }

    public async Task<IOperationResponse<IUserWishlistCardOufEntity>> Execute(IUserWishlistCardItrEntity input, CancellationToken cancellationToken)
    {
        RemoveUserWishlistCardXfrEntity xfrEntity = await _itrToXfrMapper.Map(input).ConfigureAwait(false);

        IOperationResponse<UserWishlistCardExtEntity> response = await _userWishlistCardsAdapterService.RemoveUserWishlistCardAsync(xfrEntity, cancellationToken).ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<IUserWishlistCardOufEntity>(response.OuterException);
        }

        IUserWishlistCardOufEntity mappedWishlistCard = await _extToOufMapper.Map(response.ResponseData).ConfigureAwait(false);
        return new SuccessOperationResponse<IUserWishlistCardOufEntity>(mappedWishlistCard);
    }
}
