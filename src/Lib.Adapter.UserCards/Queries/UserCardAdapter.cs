using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Gophers;
using Lib.Adapter.UserCards.Apis.Entities;
using Lib.Adapter.UserCards.Exceptions;
using Lib.Cosmos.Apis.Ids;
using Lib.Cosmos.Apis.Operators;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.UserCards.Queries;

/// <summary>
/// Retrieves a specific user card using point read operation.
/// </summary>
internal sealed class UserCardAdapter : IUserCardAdapter
{
    private readonly ICosmosGopher _userCardsGopher;

    public UserCardAdapter(ILogger logger) : this(new UserCardsGopher(logger)) { }

    private UserCardAdapter(ICosmosGopher userCardsGopher) => _userCardsGopher = userCardsGopher;

    public async Task<IOperationResponse<IEnumerable<UserCardExtEntity>>> Execute([NotNull] IUserCardXfrEntity input)
    {
        //TODO Needs a mapper
        ReadPointItem readPoint = new()
        {
            Id = new ProvidedCosmosItemId(input.CardId),
            Partition = new ProvidedPartitionKeyValue(input.UserId)
        };

        OpResponse<UserCardExtEntity> response = await _userCardsGopher.ReadAsync<UserCardExtEntity>(readPoint).ConfigureAwait(false);

        // Handle "not found" as successful with empty collection (HTTP 404 is valid for point reads)
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return new SuccessOperationResponse<IEnumerable<UserCardExtEntity>>([]);
        }

        if (response.IsNotSuccessful())
        {
            return new FailureOperationResponse<IEnumerable<UserCardExtEntity>>(
                new UserCardsAdapterException($"Failed to retrieve user card [user={input.UserId}] [card={input.CardId}]", response.Exception()));
        }

        return new SuccessOperationResponse<IEnumerable<UserCardExtEntity>>([response.Value]);
    }
}
