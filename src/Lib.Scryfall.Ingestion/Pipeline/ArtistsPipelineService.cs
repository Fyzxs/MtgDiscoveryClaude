using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Entities;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Scribes;
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
    private readonly IArtistTrigramAggregator _artistTrigramAggregator;
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
        _artistTrigramAggregator = new MonoStateArtistTrigramAggregator();
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
        List<IArtistAggregate> artists = [.. _artistAggregator.GetArtists()];
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

            // Track artist for trigram generation
            _artistTrigramAggregator.TrackArtist(artistId, artist.ArtistNames());

            // Write ArtistItem (main artist record)
            ArtistAggregateExtArg artistData = new()
            {
                ArtistId = artistId,
                ArtistNames = artist.ArtistNames(),
                ArtistNamesSearch = string.Join(" ", artist.ArtistNames()).ToLowerInvariant(),
                CardIds = artist.CardIds(),
                SetIds = artist.SetIds()
            };
            ScryfallArtistItem artistItem = new()
            {
                ArtistId = artistId,
                Data = artistData
            };
            await _artistItemsScribe.UpsertAsync(artistItem).ConfigureAwait(false);

            // Write ArtistCard relationships (artist to each card)
            foreach (dynamic card in artist.Cards())
            {
                ScryfallArtistCardExtArg artistCard = new()
                {
                    ArtistId = artistId,
                    Data = card
                };
                await _artistCardsScribe.UpsertAsync(artistCard).ConfigureAwait(false);
            }

            // Write ArtistSet relationships (artist to each set)
            foreach (string setId in artist.SetIds())
            {
                ScryfallArtistSetExtArg artistSetItem = new()
                {
                    ArtistId = artistId,
                    SetId = setId

                };
                await _artistSetsScribe.UpsertAsync(artistSetItem).ConfigureAwait(false);
            }

            // Write SetArtist relationships (set to each artist)
            foreach (string setId in artist.SetIds())
            {
                ScryfallSetArtistExtArg setArtistItem = new()
                {
                    ArtistId = artistId,
                    SetId = setId
                };
                await _setArtistsScribe.UpsertAsync(setArtistItem).ConfigureAwait(false);
            }
        }

        _dashboard.LogArtistsWritten(artistCount);
        _dashboard.UpdateCompletedCount("Artists", artistCount);

        // Write artist trigrams
        await WriteArtistTrigramsAsync().ConfigureAwait(false);
    }

    private async Task WriteArtistTrigramsAsync()
    {
        List<IArtistTrigramAggregate> trigrams = [.. _artistTrigramAggregator.GetTrigrams()];
        int trigramCount = trigrams.Count;

        if (trigramCount == 0) return;

        _dashboard.LogWritingArtistTrigrams(trigramCount);

        int current = 0;
        foreach (IArtistTrigramAggregate aggregate in trigrams)
        {
            current++;
            string trigram = aggregate.Trigram();
            _dashboard.UpdateProgress("Artist Trigrams:", current, trigramCount, "Writing Trigram", trigram);

            ArtistNameTrigramExtArg entity = new()
            {
                Trigram = aggregate.Trigram(),
                Artists = new Collection<ArtistNameTrigramDataExtArg>(
                    [.. aggregate.Entries().Select(entry => new ArtistNameTrigramDataExtArg
                    {
                        ArtistId = entry.ArtistId(),
                        Name = entry.Name(),
                        Normalized = entry.Normalized(),
                        Positions = new Collection<int>([.. entry.Positions()])
                    })])
            };

            ArtistNameTrigramsScribe trigramScribe = new(_dashboard);
            await trigramScribe.UpsertAsync(entity).ConfigureAwait(false);
        }

        _artistTrigramAggregator.Clear();
        _dashboard.LogArtistTrigramsWritten(trigramCount);
        _dashboard.UpdateCompletedCount("Artist Trigrams", trigramCount);
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

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Writing {TrigramCount} artist trigram indexes to Cosmos DB")]
    public static partial void LogWritingArtistTrigrams(this ILogger logger, int trigramCount);

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Successfully wrote {TrigramCount} artist trigram indexes to Cosmos DB")]
    public static partial void LogArtistTrigramsWritten(this ILogger logger, int trigramCount);
}
