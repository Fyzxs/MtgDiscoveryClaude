using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Lib.Adapter.Cards.Apis.Entities;
using Lib.Adapter.Cards.Queries.Mappers;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Gophers;
using Lib.Cosmos.Apis.Ids;
using Lib.Cosmos.Apis.Operators;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.Cards.Queries;

/// <summary>
/// Retrieves cards by their IDs using Cosmos DB point reads.
/// </summary>
internal sealed class CardsByIdsAdapter : ICardsByIdsAdapter
{
    private readonly ICosmosGopher _cardGopher;
    private readonly ICollectionCardIdToReadPointItemMapper _cardIdsToReadPointMapper;

    public CardsByIdsAdapter(ILogger logger) : this(
        new ScryfallCardItemsGopher(logger),
        new CollectionCardIdToReadPointItemMapper())
    { }

    private CardsByIdsAdapter(
        ICosmosGopher cardGopher,
        ICollectionCardIdToReadPointItemMapper cardIdsToReadPointMapper)
    {
        _cardGopher = cardGopher;
        _cardIdsToReadPointMapper = cardIdsToReadPointMapper;
    }

    public async Task<IOperationResponse<IEnumerable<ScryfallCardItemExtEntity>>> Execute([NotNull] ICardIdsXfrEntity input)
    {
        ICollection<ReadPointItem> items = await _cardIdsToReadPointMapper.Map(input.CardIds).ConfigureAwait(false);
        IEnumerable<Task<OpResponse<ScryfallCardItemExtEntity>>> collection = items.Select(readPointItem => _cardGopher.ReadAsync<ScryfallCardItemExtEntity>(readPointItem));

        OpResponse<ScryfallCardItemExtEntity>[] responses = await Task.WhenAll(collection).ConfigureAwait(false);

        IEnumerable<ScryfallCardItemExtEntity> successfulCards = responses
            .Where(task => task.IsSuccessful())
            .Select(task => task.Value)
            .Where(card => card is not null);

        return new SuccessOperationResponse<IEnumerable<ScryfallCardItemExtEntity>>(successfulCards);
    }
}
