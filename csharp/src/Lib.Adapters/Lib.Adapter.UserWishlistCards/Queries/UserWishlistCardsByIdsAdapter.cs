using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Gophers;
using Lib.Adapter.UserWishlistCards.Apis.Entities;
using Lib.Cosmos.Apis.Ids;
using Lib.Cosmos.Apis.Operators;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.UserWishlistCards.Queries;

/// <summary>
/// Retrieves multiple user wishlist cards using parallel point read operations.
/// </summary>
internal sealed class UserWishlistCardsByIdsAdapter : IUserWishlistCardsByIdsAdapter
{
    private readonly ICosmosGopher _userWishlistCardsGopher;

    public UserWishlistCardsByIdsAdapter(ILogger logger) : this(new UserWishlistCardsGopher(logger)) { }

    private UserWishlistCardsByIdsAdapter(ICosmosGopher userWishlistCardsGopher) => _userWishlistCardsGopher = userWishlistCardsGopher;

    public async Task<IOperationResponse<IEnumerable<UserWishlistCardExtEntity>>> Execute([NotNull] IUserWishlistCardsByIdsXfrEntity input, CancellationToken cancellationToken)
    {
        const int BatchSize = 20;
        List<UserWishlistCardExtEntity> foundCards = [];

        foreach (IEnumerable<string> batch in input.CardIds.Chunk(BatchSize))
        {
            IEnumerable<Task<OpResponse<UserWishlistCardExtEntity>>> readTasks = batch.Select(cardId =>
                _userWishlistCardsGopher.ReadAsync<UserWishlistCardExtEntity>(new ReadPointItem
                {
                    Id = new ProvidedCosmosItemId(cardId),
                    Partition = new ProvidedPartitionKeyValue(input.UserId)
                }));

            OpResponse<UserWishlistCardExtEntity>[] responses = await Task.WhenAll(readTasks).ConfigureAwait(false);

            foundCards.AddRange(responses
                .Where(r => r.StatusCode != System.Net.HttpStatusCode.NotFound && r.IsSuccessful())
                .Select(r => r.Value));
        }

        return new SuccessOperationResponse<IEnumerable<UserWishlistCardExtEntity>>(foundCards);
    }
}
