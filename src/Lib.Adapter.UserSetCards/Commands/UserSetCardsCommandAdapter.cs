using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Scribes;
using Lib.Adapter.UserSetCards.Apis;
using Lib.Adapter.UserSetCards.Apis.Entities;
using Lib.Adapter.UserSetCards.Commands.Mappers;
using Lib.Adapter.UserSetCards.Exceptions;
using Lib.Cosmos.Apis.Operators;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.UserSetCards.Commands;

/// <summary>
/// Cosmos DB implementation of the user set cards command adapter.
///
/// This class handles all Cosmos DB-specific user set cards command operations,
/// implementing the specialized IUserSetCardsCommandAdapter interface.
/// </summary>
internal sealed class UserSetCardsCommandAdapter : IUserSetCardsCommandAdapter
{
    private readonly ICosmosScribe _userSetCardsScribe;
    private readonly IUserSetCardsUpsertXfrToExtMapper _xfrToExtMapper;

    public UserSetCardsCommandAdapter(ILogger logger) : this(new UserSetCardsScribe(logger), new UserSetCardsUpsertXfrToExtMapper())
    { }

    private UserSetCardsCommandAdapter(ICosmosScribe userSetCardsScribe, IUserSetCardsUpsertXfrToExtMapper xfrToExtMapper)
    {
        _userSetCardsScribe = userSetCardsScribe;
        _xfrToExtMapper = xfrToExtMapper;
    }

    public async Task<IOperationResponse<UserSetCardExtEntity>> UpsertUserSetCardAsync(IUserSetCardUpsertXfrEntity entity)
    {
        UserSetCardExtEntity extEntity = await _xfrToExtMapper.Map(entity).ConfigureAwait(false);

        OpResponse<UserSetCardExtEntity> upsertResponse = await _userSetCardsScribe.UpsertAsync(extEntity).ConfigureAwait(false);

        if (upsertResponse.IsNotSuccessful())
        {
            return new FailureOperationResponse<UserSetCardExtEntity>(
                new UserSetCardsAdapterException($"Failed to upsert UserSetCard: {upsertResponse.StatusCode}"));
        }

        return new SuccessOperationResponse<UserSetCardExtEntity>(upsertResponse.Value);
    }
}
