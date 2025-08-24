using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.Entities;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators;
using Lib.Aggregator.Cards.Apis;
using Lib.Aggregator.Cards.Entities;
using Lib.Aggregator.Cards.Queries.Mappers;
using Lib.Cosmos.Apis.Operators;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.Cards.Queries;

internal sealed class CardAggregatorOperations : ICardAggregatorService
{
    private readonly ICosmosGopher _cardGopher;
    private readonly QueryCardsIdsToReadPointItemsMapper _mapper;
    private readonly ScryfallCardItemToCardItemItrEntityMapper _cardMapper;

    public CardAggregatorOperations(ILogger logger) : this(new ScryfallCardItemsGopher(logger), new QueryCardsIdsToReadPointItemsMapper(), new ScryfallCardItemToCardItemItrEntityMapper())
    {
    }

    private CardAggregatorOperations(ICosmosGopher cardGopher, QueryCardsIdsToReadPointItemsMapper mapper, ScryfallCardItemToCardItemItrEntityMapper cardMapper)
    {
        _cardGopher = cardGopher;
        _mapper = mapper;
        _cardMapper = cardMapper;
    }

    public async Task<IOperationResponse<ICardItemCollectionItrEntity>> CardsByIdsAsync(ICardIdsItrEntity args)
    {
        IEnumerable<ReadPointItem> readPointItems = _mapper.Map(args);
        List<Task<OpResponse<ScryfallCardItem>>> tasks = [];

        tasks.AddRange(readPointItems.Select(readPointItem => _cardGopher.ReadAsync<ScryfallCardItem>(readPointItem)));

        OpResponse<ScryfallCardItem>[] responses = await Task.WhenAll(tasks).ConfigureAwait(false);

        ICollection<ICardItemItrEntity> successfulCards = responses
            .Where(r => r.IsSuccessful())
            .Select(r => _cardMapper.Map(r.Value))
            .Where(card => card != null)
            .ToList();

        return new SuccessOperationResponse<ICardItemCollectionItrEntity>(new CardItemCollectionItrEntity { Data = successfulCards });
    }
}
