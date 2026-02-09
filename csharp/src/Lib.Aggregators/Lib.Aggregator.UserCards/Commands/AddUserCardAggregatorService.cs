using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserCards;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserSetCards;
using Lib.Adapter.UserCards.Apis;
using Lib.Adapter.UserCards.Apis.Entities;
using Lib.Adapter.UserSetCards.Apis;
using Lib.Adapter.UserSetCards.Apis.Entities;
using Lib.Aggregator.UserCards.Commands.Mappers;
using Lib.Shared.DataModels.Entities.Itrs.UserCards;
using Lib.Shared.DataModels.Entities.Oufs.UserCards;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.UserCards.Commands;

internal sealed class AddUserCardAggregatorService : IAddUserCardAggregatorService
{
    private readonly IUserCardsAdapterService _userCardsAdapterService;
    private readonly IUserSetCardsAdapterService _userSetCardsAdapterService;
    private readonly IUserCardExtToOufEntityMapper _userCardMapper;
    private readonly IAddUserCardItrToXfrMapper _addUserCardItrToXfrMapper;
    private readonly IAddUserCardXfrToAddCardToSetXfrMapper _addCardToSetMapper;
    private readonly IAddUserCardRollbackXfrMapper _rollbackMapper;

    public AddUserCardAggregatorService(ILogger logger) : this(
        new UserCardsAdapterService(logger),
        new UserSetCardsAdapterService(logger),
        new UserCardExtToOufEntityMapper(),
        new AddUserCardItrToXfrMapper(),
        new AddUserCardXfrToAddCardToSetXfrMapper(),
        new AddUserCardRollbackXfrMapper())
    { }

    private AddUserCardAggregatorService(
        IUserCardsAdapterService userCardsAdapterService,
        IUserSetCardsAdapterService userSetCardsAdapterService,
        IUserCardExtToOufEntityMapper userCardMapper,
        IAddUserCardItrToXfrMapper addUserCardItrToXfrMapper,
        IAddUserCardXfrToAddCardToSetXfrMapper addCardToSetMapper,
        IAddUserCardRollbackXfrMapper rollbackMapper)
    {
        _userCardsAdapterService = userCardsAdapterService;
        _userSetCardsAdapterService = userSetCardsAdapterService;
        _userCardMapper = userCardMapper;
        _addUserCardItrToXfrMapper = addUserCardItrToXfrMapper;
        _addCardToSetMapper = addCardToSetMapper;
        _rollbackMapper = rollbackMapper;
    }

    public async Task<IOperationResponse<IUserCardOufEntity>> Execute(
        IUserCardItrEntity input,
        CancellationToken cancellationToken)
    {
        IAddUserCardXfrEntity xfrEntity = await _addUserCardItrToXfrMapper.Map(input).ConfigureAwait(false);
        IOperationResponse<UserCardExtEntity> response = await _userCardsAdapterService.AddUserCardAsync(xfrEntity, cancellationToken).ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<IUserCardOufEntity>(response.OuterException);
        }

        IAddCardToSetXfrEntity setCardEntity = await _addCardToSetMapper.Map(xfrEntity, response.ResponseData.CollectedList).ConfigureAwait(false);
        IOperationResponse<UserSetCardExtEntity> setCardResponse = await _userSetCardsAdapterService.AddCardToSetAsync(setCardEntity, cancellationToken).ConfigureAwait(false);

        if (setCardResponse.IsFailure)
        {
            IAddUserCardXfrEntity rollbackEntity = await _rollbackMapper.Map(xfrEntity).ConfigureAwait(false);
            await _userCardsAdapterService.AddUserCardAsync(rollbackEntity, cancellationToken).ConfigureAwait(false);

            return new FailureOperationResponse<IUserCardOufEntity>(setCardResponse.OuterException);
        }

        IUserCardOufEntity mappedUserCard = await _userCardMapper.Map(response.ResponseData).ConfigureAwait(false);
        return new SuccessOperationResponse<IUserCardOufEntity>(mappedUserCard);
    }
}
