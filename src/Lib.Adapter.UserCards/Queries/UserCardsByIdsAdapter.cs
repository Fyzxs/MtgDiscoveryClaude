using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Gophers;
using Lib.Adapter.UserCards.Apis.Entities;
using Lib.Cosmos.Apis.Ids;
using Lib.Cosmos.Apis.Operators;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.UserCards.Queries;

/// <summary>
/// Retrieves multiple user cards using parallel point read operations.
/// </summary>
internal sealed class UserCardsByIdsAdapter : IUserCardsByIdsAdapter
{
    private readonly ICosmosGopher _userCardsGopher;

    public UserCardsByIdsAdapter(ILogger logger) : this(new UserCardsGopher(logger)) { }

    private UserCardsByIdsAdapter(ICosmosGopher userCardsGopher) => _userCardsGopher = userCardsGopher;

    public async Task<IOperationResponse<IEnumerable<UserCardExtEntity>>> Execute([NotNull] IUserCardsByIdsXfrEntity input)
    {
        const int BatchSize = 20;
        List<UserCardExtEntity> foundCards = [];

        // Process card IDs in batches
        foreach (IEnumerable<string> batch in input.CardIds.Chunk(BatchSize))
        {
            IEnumerable<Task<OpResponse<UserCardExtEntity>>> readTasks = batch.Select(cardId =>
                _userCardsGopher.ReadAsync<UserCardExtEntity>(new ReadPointItem
                {
                    Id = new ProvidedCosmosItemId(cardId),
                    Partition = new ProvidedPartitionKeyValue(input.UserId)
                }));

            OpResponse<UserCardExtEntity>[] responses = await Task.WhenAll(readTasks).ConfigureAwait(false);

            foundCards.AddRange(responses
                .Where(r => r.StatusCode != System.Net.HttpStatusCode.NotFound && r.IsSuccessful())
                .Select(r => r.Value));
        }

        return new SuccessOperationResponse<IEnumerable<UserCardExtEntity>>(foundCards);
    }
}
