using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserWishlistCards;
using Lib.Adapter.UserWishlistCards.Apis;
using Lib.Adapter.UserWishlistCards.Apis.Entities;
using Lib.Aggregator.UserWishlistCards.Commands.Entities;
using Lib.Aggregator.UserWishlistCards.Commands.Mappers;
using Lib.Shared.DataModels.Entities.Itrs.UserWishlistCards;
using Lib.Shared.DataModels.Entities.Oufs.UserWishlistCards;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.UserWishlistCards.Commands;

internal sealed class AddUserWishlistCardAggregator : IAddUserWishlistCardAggregator
{
    private readonly IUserWishlistCardsAdapterService _userWishlistCardsAdapterService;
    private readonly IAddUserWishlistCardItrToXfrMapper _itrToXfrMapper;
    private readonly IUserWishlistCardExtToOufEntityMapper _extToOufMapper;

    public AddUserWishlistCardAggregator(ILogger logger) : this(
        new UserWishlistCardsAdapterService(logger),
        new AddUserWishlistCardItrToXfrMapper(),
        new UserWishlistCardExtToOufEntityMapper())
    { }

    private AddUserWishlistCardAggregator(
        IUserWishlistCardsAdapterService userWishlistCardsAdapterService,
        IAddUserWishlistCardItrToXfrMapper itrToXfrMapper,
        IUserWishlistCardExtToOufEntityMapper extToOufMapper)
    {
        _userWishlistCardsAdapterService = userWishlistCardsAdapterService;
        _itrToXfrMapper = itrToXfrMapper;
        _extToOufMapper = extToOufMapper;
    }

    public async Task<IOperationResponse<IUserWishlistCardOufEntity>> Execute(IUserWishlistCardItrEntity input, CancellationToken cancellationToken)
    {
        AddUserWishlistCardXfrEntity xfrEntity = await _itrToXfrMapper.Map(input).ConfigureAwait(false);

        IOperationResponse<UserWishlistCardExtEntity> response = await _userWishlistCardsAdapterService.AddUserWishlistCardAsync(xfrEntity, cancellationToken).ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<IUserWishlistCardOufEntity>(response.OuterException);
        }

        IUserWishlistCardOufEntity mappedWishlistCard = await _extToOufMapper.Map(response.ResponseData).ConfigureAwait(false);
        return new SuccessOperationResponse<IUserWishlistCardOufEntity>(mappedWishlistCard);
    }
}
