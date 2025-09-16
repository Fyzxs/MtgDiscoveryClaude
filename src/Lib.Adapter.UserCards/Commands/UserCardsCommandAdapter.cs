using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Scribes;
using Lib.Adapter.UserCards.Apis;
using Lib.Adapter.UserCards.Commands.Mappers;
using Lib.Adapter.UserCards.Exceptions;
using Lib.Cosmos.Apis.Operators;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.UserCards.Commands;

internal sealed class UserCardsCommandAdapter : IUserCardsCommandAdapter
{
    private readonly ICosmosScribe _userCardsScribe;
    private readonly IUserCardItemMapper _userCardItemMapper;
    private readonly IUserCardCollectionItrEntityMapper _userCardCollectionMapper;

    public UserCardsCommandAdapter(ILogger logger) : this(new UserCardsScribe(logger), new UserCardItemMapper(), new UserCardCollectionItrEntityMapper())
    { }

    internal UserCardsCommandAdapter(ICosmosScribe userCardsScribe, IUserCardItemMapper userCardItemMapper, IUserCardCollectionItrEntityMapper userCardCollectionMapper)
    {
        _userCardsScribe = userCardsScribe;
        _userCardItemMapper = userCardItemMapper;
        _userCardCollectionMapper = userCardCollectionMapper;
    }

    public async Task<IOperationResponse<IUserCardCollectionItrEntity>> AddUserCardAsync(IUserCardCollectionItrEntity userCard)
    {
        UserCardItem userCardItem = await _userCardItemMapper.Map(userCard).ConfigureAwait(false);
        OpResponse<UserCardItem> cosmosResponse = await _userCardsScribe.UpsertAsync(userCardItem).ConfigureAwait(false);

        if (cosmosResponse.IsNotSuccessful()) return new FailureOperationResponse<IUserCardCollectionItrEntity>(new UserCardsAdapterException($"Failed to add user card: {cosmosResponse.StatusCode}"));

        IUserCardCollectionItrEntity resultEntity = await _userCardCollectionMapper.Map(cosmosResponse.Value).ConfigureAwait(false);
        return new SuccessOperationResponse<IUserCardCollectionItrEntity>(resultEntity);
    }
}
