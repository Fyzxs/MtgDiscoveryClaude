using System.Linq;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Scribes;
using Lib.Scryfall.Ingestion.Apis.Aggregation;
using Microsoft.Extensions.Logging;

namespace Lib.Scryfall.Ingestion.Processors;

internal sealed class ArtistSetsProcessor : IArtistProcessor
{
    private readonly ScryfallArtistSetsScribe _scribe;
    private readonly ILogger _logger;

    public ArtistSetsProcessor(ILogger logger)
        : this(new ScryfallArtistSetsScribe(logger), logger)
    {
    }

    private ArtistSetsProcessor(ScryfallArtistSetsScribe scribe, ILogger logger)
    {
        _scribe = scribe;
        _logger = logger;
    }

    public async Task ProcessAsync(IArtistAggregate artist)
    {
        string artistId = artist.ArtistId();

        foreach (string setId in artist.SetIds())
        {
            ScryfallArtistSetExtArg item = new()
            {
                ArtistId = artistId,
                SetId = setId
            };
            await _scribe.UpsertAsync(item).ConfigureAwait(false);
        }

        _logger.LogArtistSetsWritten(artistId, artist.SetIds().Count());
    }
}

internal static partial class ArtistSetsProcessorLoggerExtensions
{
    [LoggerMessage(
        Level = LogLevel.Debug,
        Message = "Written {SetCount} artist-set relationships for artist {ArtistId}")]
    public static partial void LogArtistSetsWritten(this ILogger logger, string artistId, int setCount);
}
