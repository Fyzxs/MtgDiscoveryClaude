using System.Linq;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Scribes;
using Lib.Scryfall.Ingestion.Apis.Aggregation;

namespace Lib.Scryfall.Ingestion.Processors;

internal sealed class ArtistCardsWriterProcessor : IArtistProcessor
{
    private readonly ScryfallArtistCardsScribe _scribe;
    private readonly ILogger _logger;

    public ArtistCardsWriterProcessor(ILogger logger)
        : this(new ScryfallArtistCardsScribe(logger), logger)
    {
    }

    private ArtistCardsWriterProcessor(ScryfallArtistCardsScribe scribe, ILogger logger)
    {
        _scribe = scribe;
        _logger = logger;
    }

    public async Task ProcessAsync(IArtistAggregate artist)
    {
        string artistId = artist.ArtistId();

        foreach (dynamic cardData in artist.Cards())
        {
            ScryfallArtistCardExtEntity item = new()
            {
                ArtistId = artistId,
                Data = cardData
            };
            await _scribe.UpsertAsync(item).ConfigureAwait(false);
        }

        _logger.LogArtistCardsWritten(artistId, artist.CardIds().Count());
    }
}

internal static partial class ArtistCardsWriterProcessorLoggerExtensions
{
    [LoggerMessage(
        Level = LogLevel.Debug,
        Message = "Written {CardCount} artist-card relationships for artist {ArtistId}")]
    public static partial void LogArtistCardsWritten(this ILogger logger, string artistId, int cardCount);
}
