using System.Linq;
using System.Threading.Tasks;
using Lib.Scryfall.Ingestion.Aggregation;
using Lib.Scryfall.Ingestion.Apis.Aggregation;
using Lib.Scryfall.Shared.Apis.Models;

namespace Lib.Scryfall.Ingestion.Processors;

internal sealed class ArtistAggregatorProcessor : ICardProcessor
{
    private readonly IArtistAggregator _aggregator;
    private readonly ILogger _logger;

    public ArtistAggregatorProcessor(ILogger logger)
        : this(new MonoStateArtistAggregator(), logger)
    {
    }

    private ArtistAggregatorProcessor(IArtistAggregator aggregator, ILogger logger)
    {
        _aggregator = aggregator;
        _logger = logger;
    }

    public Task ProcessAsync(IScryfallCard card)
    {
        _aggregator.Track(card);
        _logger.LogArtistDataAggregated(card.Id(), card.ArtistIds().Count());
        return Task.CompletedTask;
    }
}

internal static partial class ArtistAggregatorProcessorLoggerExtensions
{
    [LoggerMessage(
        Level = LogLevel.Debug,
        Message = "Aggregated artist data for card {CardId} with {ArtistCount} artist(s)")]
    public static partial void LogArtistDataAggregated(this ILogger logger, string cardId, int artistCount);
}
