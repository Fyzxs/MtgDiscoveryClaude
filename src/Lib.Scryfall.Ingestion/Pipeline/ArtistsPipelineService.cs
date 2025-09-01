using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.Dtos;
using Lib.Adapter.Scryfall.Cosmos.Apis.Entities;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators;
using Lib.Scryfall.Ingestion.Aggregation;
using Lib.Scryfall.Ingestion.Apis.Aggregation;
using Lib.Scryfall.Ingestion.Apis.Dashboard;
using Lib.Scryfall.Ingestion.Apis.Pipeline;
using Lib.Scryfall.Shared.Apis.Models;
using Microsoft.Extensions.Logging;

namespace Lib.Scryfall.Ingestion.Pipeline;

internal sealed class ArtistsPipelineService : IArtistsPipelineService
{
    private readonly IArtistAggregator _artistAggregator;
    private readonly ScryfallArtistItemsScribe _artistItemsScribe;
    private readonly ScryfallArtistCardsScribe _artistCardsScribe;
    private readonly ScryfallArtistSetsScribe _artistSetsScribe;
    private readonly ScryfallSetArtistsScribe _setArtistsScribe;
    private readonly IIngestionDashboard _dashboard;

    public ArtistsPipelineService(
        IIngestionDashboard dashboard,
        ILogger logger)
    {
        _artistAggregator = new MonoStateArtistAggregator();
        _artistItemsScribe = new ScryfallArtistItemsScribe(logger);
        _artistCardsScribe = new ScryfallArtistCardsScribe(logger);
        _artistSetsScribe = new ScryfallArtistSetsScribe(logger);
        _setArtistsScribe = new ScryfallSetArtistsScribe(logger);
        _dashboard = dashboard;
    }

    public void TrackArtist(IScryfallCard card)
    {
        _artistAggregator.Track(card);
    }

    public async Task WriteArtistsAsync()
    {
        List<IArtistAggregate> artists = _artistAggregator.GetArtists().ToList();
        int artistCount = artists.Count;

        if (artistCount == 0) return;

        _dashboard.LogWritingArtists(artistCount);

        int current = 0;
        foreach (IArtistAggregate artist in artists)
        {
            current++;
            string artistId = artist.ArtistId();
            string displayName = artist.ArtistNames().FirstOrDefault() ?? artistId;
            _dashboard.UpdateProgress("Artists:", current, artistCount, "Writing", displayName);

            // Write ArtistItem (main artist record)
            ArtistAggregateData artistData = new()
            {
                ArtistId = artistId,
                ArtistNames = artist.ArtistNames(),
                ArtistNamesSearch = string.Join(" ", artist.ArtistNames()).ToLowerInvariant(),
                CardIds = artist.CardIds(),
                SetIds = artist.SetIds()
            };
            ScryfallArtistItem artistItem = new(artistId, artistData);
            await _artistItemsScribe.UpsertAsync(artistItem).ConfigureAwait(false);

            // Write ArtistCard relationships (artist to each card)
            foreach (dynamic card in artist.Cards())
            {
                ScryfallArtistCard artistCard = new()
                {
                    ArtistId = artistId,
                    Data = card
                };
                await _artistCardsScribe.UpsertAsync(artistCard).ConfigureAwait(false);
            }

            // Write ArtistSet relationships (artist to each set)
            foreach (string setId in artist.SetIds())
            {
                ScryfallArtistSet artistSet = new(setId, artistId);
                await _artistSetsScribe.UpsertAsync(artistSet).ConfigureAwait(false);
            }

            // Write SetArtist relationships (set to each artist)
            foreach (string setId in artist.SetIds())
            {
                ScryfallSetArtist setArtist = new(artistId, setId);
                await _setArtistsScribe.UpsertAsync(setArtist).ConfigureAwait(false);
            }
        }

        _dashboard.LogArtistsWritten(artistCount);
    }
}

internal static partial class ArtistsPipelineServiceLoggerExtensions
{
    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Extracted {Count} unique artists")]
    public static partial void LogArtistsExtracted(this ILogger logger, int count);

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Writing {Count} artists to Cosmos DB")]
    public static partial void LogWritingArtists(this ILogger logger, int count);

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Successfully wrote {Count} artists to Cosmos DB")]
    public static partial void LogArtistsWritten(this ILogger logger, int count);
}