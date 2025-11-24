using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.UserSetCards.Apis;
using Lib.Adapter.UserSetCards.Apis.Entities;
using Lib.Aggregator.UserSetCards.Commands.Mappers;
using Lib.Aggregator.UserSetCards.Queries.Mappers;
using Lib.Shared.DataModels.Entities.Itrs.UserSetCards;
using Lib.Shared.DataModels.Entities.Oufs.UserSetCards;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.UserSetCards.Commands;

/// <summary>
/// Aggregator service that adds a card to a user's set collection.
/// Used by migration tools to directly update UserSetCards without affecting UserCards.
/// </summary>
internal sealed class AddCardToSetAggregatorService : IAddCardToSetAggregatorService
{
    private readonly IUserSetCardsAdapterService _userSetCardsAdapterService;
    private readonly IAddCardToSetItrToXfrMapper _itrToXfrMapper;
    private readonly IUserSetCardExtToItrMapper _extToItrMapper;

    public AddCardToSetAggregatorService(ILogger logger) : this(
        new UserSetCardsAdapterService(logger),
        new AddCardToSetItrToXfrMapper(),
        new UserSetCardExtToItrMapper())
    { }

    private AddCardToSetAggregatorService(
        IUserSetCardsAdapterService userSetCardsAdapterService,
        IAddCardToSetItrToXfrMapper itrToXfrMapper,
        IUserSetCardExtToItrMapper extToItrMapper)
    {
        _userSetCardsAdapterService = userSetCardsAdapterService;
        _itrToXfrMapper = itrToXfrMapper;
        _extToItrMapper = extToItrMapper;
    }

    public async Task<IOperationResponse<IUserSetCardOufEntity>> Execute(IAddCardToSetItrEntity input)
    {
        IAddCardToSetXfrEntity xfrEntity = await _itrToXfrMapper.Map(input).ConfigureAwait(false);
        IOperationResponse<UserSetCardExtEntity> response = await _userSetCardsAdapterService.AddCardToSetAsync(xfrEntity).ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<IUserSetCardOufEntity>(response.OuterException);
        }

        IUserSetCardOufEntity mappedUserSetCard = await _extToItrMapper.Map(response.ResponseData).ConfigureAwait(false);
        return new SuccessOperationResponse<IUserSetCardOufEntity>(mappedUserSetCard);
    }
}
