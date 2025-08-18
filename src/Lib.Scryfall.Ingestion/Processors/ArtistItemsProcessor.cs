using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.Dtos;
using Lib.Adapter.Scryfall.Cosmos.Apis.Entities;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators;
using Lib.Scryfall.Ingestion.Apis.Aggregation;
using Microsoft.Extensions.Logging;

namespace Lib.Scryfall.Ingestion.Processors;

internal sealed class ArtistItemsProcessor : IArtistProcessor
{
    private readonly ScryfallArtistItemsScribe _scribe;
    private readonly ILogger _logger;

    public ArtistItemsProcessor(ILogger logger)
        : this(new ScryfallArtistItemsScribe(logger), logger)
    {
    }

    private ArtistItemsProcessor(ScryfallArtistItemsScribe scribe, ILogger logger)
    {
        _scribe = scribe;
        _logger = logger;
    }

    public async Task ProcessAsync(IArtistAggregate artist)
    {
        string artistId = artist.ArtistId();

        ArtistAggregateData data = new()
        {
            ArtistId = artistId,
            ArtistNames = artist.ArtistNames(),
            CardIds = artist.CardIds(),
            SetIds = artist.SetIds()
        };

        ScryfallArtistItem item = new(artistId, data);
        await _scribe.UpsertAsync(item).ConfigureAwait(false);
        _logger.LogArtistItemWritten(artistId);
    }
}

internal static partial class ArtistItemsProcessorLoggerExtensions
{
    [LoggerMessage(
        Level = LogLevel.Debug,
        Message = "Written artist item for artist {ArtistId}")]
    public static partial void LogArtistItemWritten(this ILogger logger, string artistId);
}
