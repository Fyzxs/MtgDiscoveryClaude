using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Gophers;
using Lib.Adapter.UserSetCards.Apis;
using Lib.Adapter.UserSetCards.Apis.Entities;
using Lib.Adapter.UserSetCards.Exceptions;
using Lib.Adapter.UserSetCards.Queries.Mappers;
using Lib.Cosmos.Apis.Operators;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.UserSetCards.Queries;

/// <summary>
/// Cosmos DB implementation of the user set card query adapter.
///
/// This class handles all Cosmos DB-specific user set card query operations,
/// implementing the specialized IUserSetCardQueryAdapter interface.
/// </summary>
internal sealed class UserSetCardsQueryAdapter : IUserSetCardsQueryAdapter
{
    private readonly ICosmosGopher _userSetCardsGopher;
    private readonly IUserSetCardsGetXfrToExtMapper _readPointMapper;

    public UserSetCardsQueryAdapter(ILogger logger) : this(new UserSetCardsGopher(logger), new UserSetCardsGetXfrToExtMapper())
    { }

    private UserSetCardsQueryAdapter(ICosmosGopher userSetCardsGopher, IUserSetCardsGetXfrToExtMapper readPointMapper)
    {
        _userSetCardsGopher = userSetCardsGopher;
        _readPointMapper = readPointMapper;
    }

    public async Task<IOperationResponse<UserSetCardExtEntity>> GetUserSetCardAsync(IUserSetCardGetXfrEntity readParams)
    {
        ReadPointItem readPoint = await _readPointMapper.Map(readParams).ConfigureAwait(false);

        OpResponse<UserSetCardExtEntity> readResponse = await _userSetCardsGopher.ReadAsync<UserSetCardExtEntity>(readPoint).ConfigureAwait(false);

        if (readResponse.IsNotSuccessful())
        {
            return new FailureOperationResponse<UserSetCardExtEntity>(
                new UserSetCardsAdapterException($"UserSetCard not found for userId: {readParams.UserId}, setId: {readParams.SetId}"));
        }

        return new SuccessOperationResponse<UserSetCardExtEntity>(readResponse.Value);
    }
}
