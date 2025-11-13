using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.UserCards.Apis;
using Lib.Adapter.UserCards.Apis.Entities;
using Lib.Aggregator.UserCards.Commands.Mappers;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.UserCards.Commands;

/// <summary>
/// Aggregator service that adds a user card without updating UserSetCards.
/// Used by migration tools to separate individual card tracking from set-level aggregation.
/// </summary>
internal sealed class AddUserCardOnlyAggregatorService : IAddUserCardOnlyAggregatorService
{
    private readonly IUserCardsAdapterService _userCardsAdapterService;
    private readonly IUserCardExtToItrEntityMapper _userCardMapper;
    private readonly IAddUserCardItrToXfrMapper _addUserCardItrToXfrMapper;

    public AddUserCardOnlyAggregatorService(ILogger logger) : this(
        new UserCardsAdapterService(logger),
        new UserCardExtToItrEntityMapper(),
        new AddUserCardItrToXfrMapper())
    { }

    private AddUserCardOnlyAggregatorService(
        IUserCardsAdapterService userCardsAdapterService,
        IUserCardExtToItrEntityMapper userCardMapper,
        IAddUserCardItrToXfrMapper addUserCardItrToXfrMapper)
    {
        _userCardsAdapterService = userCardsAdapterService;
        _userCardMapper = userCardMapper;
        _addUserCardItrToXfrMapper = addUserCardItrToXfrMapper;
    }

    public async Task<IOperationResponse<IUserCardOufEntity>> Execute(IUserCardItrEntity input)
    {
        IAddUserCardXfrEntity xfrEntity = await _addUserCardItrToXfrMapper.Map(input).ConfigureAwait(false);
        IOperationResponse<UserCardExtEntity> response = await _userCardsAdapterService.AddUserCardAsync(xfrEntity).ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<IUserCardOufEntity>(response.OuterException);
        }

        IUserCardOufEntity mappedUserCard = await _userCardMapper.Map(response.ResponseData).ConfigureAwait(false);
        return new SuccessOperationResponse<IUserCardOufEntity>(mappedUserCard);
    }
}
