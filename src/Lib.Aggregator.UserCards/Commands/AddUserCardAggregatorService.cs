using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.UserCards.Apis;
using Lib.Adapter.UserCards.Apis.Entities;
using Lib.Adapter.UserSetCards.Apis;
using Lib.Adapter.UserSetCards.Apis.Entities;
using Lib.Aggregator.UserCards.Commands.Mappers;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.UserCards.Commands;

internal sealed class AddUserCardAggregatorService : IAddUserCardAggregatorService
{
    private readonly IUserCardsAdapterService _userCardsAdapterService;
    private readonly IUserSetCardsAdapterService _userSetCardsAdapterService;
    private readonly IUserCardExtToItrEntityMapper _userCardMapper;
    private readonly IAddUserCardItrToXfrMapper _addUserCardItrToXfrMapper;
    private readonly IAddUserCardXfrToAddCardToSetXfrMapper _addCardToSetMapper;

    public AddUserCardAggregatorService(ILogger logger) : this(
        new UserCardsAdapterService(logger),
        new UserSetCardsAdapterService(logger),
        new UserCardExtToItrEntityMapper(),
        new AddUserCardItrToXfrMapper(),
        new AddUserCardXfrToAddCardToSetXfrMapper())
    { }

    private AddUserCardAggregatorService(
        IUserCardsAdapterService userCardsAdapterService,
        IUserSetCardsAdapterService userSetCardsAdapterService,
        IUserCardExtToItrEntityMapper userCardMapper,
        IAddUserCardItrToXfrMapper addUserCardItrToXfrMapper,
        IAddUserCardXfrToAddCardToSetXfrMapper addCardToSetMapper)
    {
        _userCardsAdapterService = userCardsAdapterService;
        _userSetCardsAdapterService = userSetCardsAdapterService;
        _userCardMapper = userCardMapper;
        _addUserCardItrToXfrMapper = addUserCardItrToXfrMapper;
        _addCardToSetMapper = addCardToSetMapper;
    }

    public async Task<IOperationResponse<IUserCardOufEntity>> Execute(IUserCardItrEntity input)
    {
        IAddUserCardXfrEntity xfrEntity = await _addUserCardItrToXfrMapper.Map(input).ConfigureAwait(false);
        IOperationResponse<UserCardExtEntity> response = await _userCardsAdapterService.AddUserCardAsync(xfrEntity).ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<IUserCardOufEntity>(response.OuterException);
        }

        IAddCardToSetXfrEntity setCardEntity = await _addCardToSetMapper.Map(xfrEntity).ConfigureAwait(false);
        IOperationResponse<UserSetCardExtEntity> setCardResponse = await _userSetCardsAdapterService.AddCardToSetAsync(setCardEntity).ConfigureAwait(false);

        if (setCardResponse.IsFailure)
        {
            IAddUserCardXfrEntity rollbackEntity = CreateRollbackEntity(xfrEntity);
            await _userCardsAdapterService.AddUserCardAsync(rollbackEntity).ConfigureAwait(false);

            return new FailureOperationResponse<IUserCardOufEntity>(setCardResponse.OuterException);
        }

        IUserCardOufEntity mappedUserCard = await _userCardMapper.Map(response.ResponseData).ConfigureAwait(false);
        return new SuccessOperationResponse<IUserCardOufEntity>(mappedUserCard);
    }

    private static IAddUserCardXfrEntity CreateRollbackEntity(IAddUserCardXfrEntity original)
    {
        UserCardDetailsXfrEntity negatedDetails = new()
        {
            Finish = original.Details.Finish,
            Special = original.Details.Special,
            Count = -original.Details.Count,
            SetGroupId = original.Details.SetGroupId
        };

        return new AddUserCardXfrEntity
        {
            UserId = original.UserId,
            CardId = original.CardId,
            SetId = original.SetId,
            ArtistIds = original.ArtistIds,
            CardNameGuid = original.CardNameGuid,
            Details = negatedDetails
        };
    }

    private sealed class AddUserCardXfrEntity : IAddUserCardXfrEntity
    {
        public required string UserId { get; init; }
        public required string CardId { get; init; }
        public required string SetId { get; init; }
        public required IEnumerable<string> ArtistIds { get; init; }
        public required string CardNameGuid { get; init; }
        public required IUserCardDetailsXfrEntity Details { get; init; }
    }

    private sealed class UserCardDetailsXfrEntity : IUserCardDetailsXfrEntity
    {
        public required string Finish { get; init; }
        public required string Special { get; init; }
        public required int Count { get; init; }
        public required string SetGroupId { get; init; }
    }

}
