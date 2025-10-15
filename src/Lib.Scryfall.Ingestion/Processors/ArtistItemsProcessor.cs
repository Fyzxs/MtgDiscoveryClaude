using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Entities;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Scribes;
using Lib.Scryfall.Ingestion.Apis.Aggregation;
using Lib.Scryfall.Ingestion.Internal.Text;

namespace Lib.Scryfall.Ingestion.Processors;

internal sealed class ArtistItemsProcessor : IArtistProcessor
{
    private readonly ScryfallArtistItemsScribe _scribe;
    private readonly IArtistNameSearchNormalizer _normalizer;
    private readonly ILogger _logger;

    public ArtistItemsProcessor(ILogger logger)
        : this(new ScryfallArtistItemsScribe(logger), new ArtistNameSearchNormalizer(), logger)
    {
    }

    private ArtistItemsProcessor(ScryfallArtistItemsScribe scribe, IArtistNameSearchNormalizer normalizer, ILogger logger)
    {
        _scribe = scribe;
        _normalizer = normalizer;
        _logger = logger;
    }

    public async Task ProcessAsync(IArtistAggregate artist)
    {
        string artistId = artist.ArtistId();
        IEnumerable<string> artistNames = [.. artist.ArtistNames()];

        ArtistAggregateExtEntity data = new()
        {
            ArtistId = artistId,
            ArtistNames = artistNames,
            ArtistNamesSearch = _normalizer.Normalize(artistNames),
            CardIds = artist.CardIds(),
            SetIds = artist.SetIds()
        };

        ScryfallArtistItem item = new()
        {
            ArtistId = artistId,
            Data = data
        };
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
