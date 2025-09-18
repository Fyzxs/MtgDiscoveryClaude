using System.Linq;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Scribes;
using Lib.Scryfall.Ingestion.Apis.Aggregation;
using Microsoft.Extensions.Logging;

namespace Lib.Scryfall.Ingestion.Processors;

internal sealed class SetArtistsProcessor : IArtistProcessor
{
    private readonly ScryfallSetArtistsScribe _scribe;
    private readonly ILogger _logger;

    public SetArtistsProcessor(ILogger logger)
        : this(new ScryfallSetArtistsScribe(logger), logger)
    {
    }

    private SetArtistsProcessor(ScryfallSetArtistsScribe scribe, ILogger logger)
    {
        _scribe = scribe;
        _logger = logger;
    }

    public async Task ProcessAsync(IArtistAggregate artist)
    {
        string artistId = artist.ArtistId();

        foreach (string setId in artist.SetIds())
        {
            ScryfallSetArtistExtEntity item = new()
            {
                ArtistId = artistId,
                SetId = setId
            };
            await _scribe.UpsertAsync(item).ConfigureAwait(false);
        }

        _logger.LogSetArtistsWritten(artistId, artist.SetIds().Count());
    }
}

internal static partial class SetArtistsProcessorLoggerExtensions
{
    [LoggerMessage(
        Level = LogLevel.Debug,
        Message = "Written {SetCount} set-artist relationships for artist {ArtistId}")]
    public static partial void LogSetArtistsWritten(this ILogger logger, string artistId, int setCount);
}
