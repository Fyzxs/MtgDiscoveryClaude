using System.Threading.Tasks;
using Lib.Scryfall.Ingestion.Aggregation;
using Lib.Scryfall.Ingestion.Apis.Aggregation;
using Lib.Scryfall.Shared.Apis.Models;
using Microsoft.Extensions.Logging;

namespace Lib.Scryfall.Ingestion.Processors;

internal sealed class CardNameTrigramAggregatorProcessor : ICardProcessor
{
    private readonly ICardNameTrigramAggregator _aggregator;
    private readonly ILogger _logger;

    public CardNameTrigramAggregatorProcessor(ILogger logger)
        : this(new MonoStateCardNameTrigramAggregator(), logger)
    {
    }

    private CardNameTrigramAggregatorProcessor(ICardNameTrigramAggregator aggregator, ILogger logger)
    {
        _aggregator = aggregator;
        _logger = logger;
    }

    public Task ProcessAsync(IScryfallCard card)
    {
        _aggregator.Track(card);
        return Task.CompletedTask;
    }
}
