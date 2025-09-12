using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Nesteds;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Scribes;
using Lib.Scryfall.Ingestion.Aggregation;
using Lib.Scryfall.Ingestion.Apis.Aggregation;
using Microsoft.Extensions.Logging;

namespace Lib.Scryfall.Ingestion.Processors;

internal interface ICardNameTrigramWriterProcessor
{
    Task WriteTrigramsAsync();
}

internal sealed class CardNameTrigramWriterProcessor : ICardNameTrigramWriterProcessor
{
    private readonly ICardNameTrigramAggregator _aggregator;
    private readonly CardNameTrigramsScribe _scribe;
    private readonly ILogger _logger;

    public CardNameTrigramWriterProcessor(ILogger logger)
        : this(
            new MonoStateCardNameTrigramAggregator(),
            new CardNameTrigramsScribe(logger),
            logger)
    {
    }

    private CardNameTrigramWriterProcessor(
        ICardNameTrigramAggregator aggregator,
        CardNameTrigramsScribe scribe,
        ILogger logger)
    {
        _aggregator = aggregator;
        _scribe = scribe;
        _logger = logger;
    }

    public async Task WriteTrigramsAsync()
    {
        _logger.LogCardNameTrigramWriteStarted();

        int trigramCount = 0;
        foreach (ICardNameTrigramAggregate aggregate in _aggregator.GetTrigrams())
        {
            CardNameTrigramItem entity = new()
            {
                Trigram = aggregate.Trigram(),
                Cards = new Collection<CardNameTrigramDataItem>(
                    aggregate.Entries().Select(entry => new CardNameTrigramDataItem
                    {
                        Name = entry.Name(),
                        Normalized = entry.Normalized(),
                        Positions = new Collection<int>(entry.Positions().ToList())
                    }).ToList())
            };

            await _scribe.UpsertAsync(entity).ConfigureAwait(false);
            trigramCount++;
        }

        _logger.LogCardNameTrigramWriteCompleted(trigramCount);

        // Clear the aggregator after writing
        _aggregator.Clear();
    }
}

internal static partial class CardNameTrigramWriterProcessorLoggerExtensions
{
    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Starting to write card name trigram indexes to Cosmos DB")]
    public static partial void LogCardNameTrigramWriteStarted(this ILogger logger);

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Completed writing {TrigramCount} trigram indexes to Cosmos DB")]
    public static partial void LogCardNameTrigramWriteCompleted(this ILogger logger, int trigramCount);
}
