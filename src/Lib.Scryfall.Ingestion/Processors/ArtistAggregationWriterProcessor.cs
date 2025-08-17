using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.Scryfall.Ingestion.Aggregation;
using Lib.Scryfall.Ingestion.Apis.Aggregation;
using Microsoft.Extensions.Logging;

namespace Lib.Scryfall.Ingestion.Processors;

internal sealed class ArtistAggregationWriterProcessor : IArtistAggregateProcessor
{
    private readonly IArtistAggregator _aggregator;
    private readonly IReadOnlyList<IArtistProcessor> _processors;
    private readonly ILogger _logger;

    public ArtistAggregationWriterProcessor(ILogger logger)
        : this(
            new MonoStateArtistAggregator(),
            [
                new ArtistItemsProcessor(logger),
                new ArtistCardsWriterProcessor(logger),
                new ArtistSetsProcessor(logger),
                new SetArtistsProcessor(logger)
            ],
            logger)
    {
    }

    private ArtistAggregationWriterProcessor(
        IArtistAggregator aggregator,
        IReadOnlyList<IArtistProcessor> processors,
        ILogger logger)
    {
        _aggregator = aggregator;
        _processors = processors;
        _logger = logger;
    }

    public async Task ProcessAsync()
    {
        List<IArtistAggregate> artists = _aggregator.GetArtists().ToList();

        foreach (IArtistAggregate artist in artists)
        {
            foreach (IArtistProcessor processor in _processors)
            {
                await processor.ProcessAsync(artist).ConfigureAwait(false);
            }
        }

        _logger.LogArtistAggregationWriteCompleted(artists.Count);
    }
}

internal static partial class ArtistAggregationWriterProcessorLoggerExtensions
{
    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Completed writing artist aggregation data for {ArtistCount} artists")]
    public static partial void LogArtistAggregationWriteCompleted(this ILogger logger, int artistCount);
}
