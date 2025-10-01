using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Entities;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Gophers;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Scribes;
using Lib.Cosmos.Apis.Ids;
using Lib.Cosmos.Apis.Operators;
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
    private readonly ICosmosGopher _artistItemsGopher;
    private readonly ICosmosGopher _artistTrigramGopher;
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
        _artistItemsGopher = new ScryfallArtistItemsGopher(logger);
        _artistTrigramGopher = new ArtistNameTrigramsGopher(logger);
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

            // Write ArtistItem (main artist record) with merged data
            ArtistAggregateExtEntity artistData = await GetMergedArtistDataAsync(artistId, artist).ConfigureAwait(false);

            ScryfallArtistItem artistItem = new()
            {
                ArtistId = artistId,
                Data = artistData
            };
            _ = await _artistItemsScribe.UpsertAsync(artistItem).ConfigureAwait(false);

            // Write ArtistCard relationships (artist to each card)
            foreach (dynamic card in artist.Cards())
            {
                ScryfallArtistCardExtEntity artistCard = new()
                {
                    ArtistId = artistId,
                    Data = card
                };
                _ = await _artistCardsScribe.UpsertAsync(artistCard).ConfigureAwait(false);
            }

            // Write ArtistSet relationships (artist to each set)
            foreach (string setId in artist.SetIds())
            {
                ScryfallArtistSetExtEntity artistSetItem = new()
                {
                    ArtistId = artistId,
                    SetId = setId

                };
                _ = await _artistSetsScribe.UpsertAsync(artistSetItem).ConfigureAwait(false);
            }

            // Write SetArtist relationships (set to each artist)
            foreach (string setId in artist.SetIds())
            {
                ScryfallSetArtistExtEntity setArtistItem = new()
                {
                    ArtistId = artistId,
                    SetId = setId
                };
                _ = await _setArtistsScribe.UpsertAsync(setArtistItem).ConfigureAwait(false);
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

            ArtistNameTrigramExtEntity entity = await GetMergedArtistTrigramDataAsync(
                aggregate.Trigram(),
                aggregate).ConfigureAwait(false);

            ArtistNameTrigramsScribe trigramScribe = new(_dashboard);
            _ = await trigramScribe.UpsertAsync(entity).ConfigureAwait(false);
        }

        _artistTrigramAggregator.Clear();
        _dashboard.LogArtistTrigramsWritten(trigramCount);
        _dashboard.UpdateCompletedCount("Artist Trigrams", trigramCount);
    }

    private async Task<ArtistAggregateExtEntity> GetMergedArtistDataAsync(
        string artistId,
        IArtistAggregate newArtistData)
    {
        // Try to read existing artist data
        ReadPointItem readPoint = new()
        {
            Id = new ProvidedCosmosItemId(artistId),
            Partition = new ProvidedPartitionKeyValue(artistId)
        };

        OpResponse<ScryfallArtistItem> existingResponse =
            await _artistItemsGopher.ReadAsync<ScryfallArtistItem>(readPoint).ConfigureAwait(false);

        if (existingResponse.IsNotSuccessful())
        {
            // No existing data, create fresh
            return new ArtistAggregateExtEntity
            {
                ArtistId = artistId,
                ArtistNames = newArtistData.ArtistNames(),
                ArtistNamesSearch = string.Join(" ", newArtistData.ArtistNames()).ToLowerInvariant(),
                CardIds = newArtistData.CardIds(),
                SetIds = newArtistData.SetIds()
            };
        }

        // Merge existing with new
        ArtistAggregateExtEntity existingData = existingResponse.Value.Data;

        // Merge CardIds (union of existing + new)
        IEnumerable<string> mergedCardIds = [.. existingData.CardIds.Union(newArtistData.CardIds())];

        // Merge SetIds (union of existing + new)
        IEnumerable<string> mergedSetIds = [.. existingData.SetIds.Union(newArtistData.SetIds())];

        // Merge ArtistNames (union of existing + new, preserving order)
        IEnumerable<string> mergedNames = [.. existingData.ArtistNames.Union(newArtistData.ArtistNames())];

        return new ArtistAggregateExtEntity
        {
            ArtistId = artistId,
            ArtistNames = mergedNames,
            ArtistNamesSearch = string.Join(" ", mergedNames).ToLowerInvariant(),
            CardIds = mergedCardIds,
            SetIds = mergedSetIds
        };
    }

    private async Task<ArtistNameTrigramExtEntity> GetMergedArtistTrigramDataAsync(
        string trigram,
        IArtistTrigramAggregate newTrigramData)
    {
        // Try to read existing trigram data
        ReadPointItem readPoint = new()
        {
            Id = new ProvidedCosmosItemId(trigram),
            Partition = new ProvidedPartitionKeyValue(trigram[..1])
        };

        OpResponse<ArtistNameTrigramExtEntity> existingResponse =
            await _artistTrigramGopher.ReadAsync<ArtistNameTrigramExtEntity>(readPoint).ConfigureAwait(false);

        Collection<ArtistNameTrigramDataExtEntity> newArtists = new(
            [.. newTrigramData.Entries().Select(entry => new ArtistNameTrigramDataExtEntity
            {
                ArtistId = entry.ArtistId(),
                Name = entry.Name(),
                Normalized = entry.Normalized(),
                Positions = new Collection<int>([.. entry.Positions()])
            })]);

        if (existingResponse.IsNotSuccessful())
        {
            // No existing data, create fresh
            return new ArtistNameTrigramExtEntity
            {
                Trigram = trigram,
                Artists = newArtists
            };
        }

        // Merge existing with new
        ArtistNameTrigramExtEntity existingData = existingResponse.Value;

        // Create lookup by artist ID + normalized name to avoid duplicates
        // Use GroupBy to handle corrupt data with duplicates, taking first entry
        Dictionary<string, ArtistNameTrigramDataExtEntity> artistsByKey = existingData.Artists
            .GroupBy(a => $"{a.ArtistId}:{a.Normalized}")
            .ToDictionary(g => g.Key, g => g.First());

        // Add or update with new artists
        foreach (ArtistNameTrigramDataExtEntity newArtist in newArtists)
        {
            string key = $"{newArtist.ArtistId}:{newArtist.Normalized}";
            // Upsert: if exists, replace with new data; if not, add
            artistsByKey[key] = newArtist;
        }

        return new ArtistNameTrigramExtEntity
        {
            Trigram = trigram,
            Artists = new Collection<ArtistNameTrigramDataExtEntity>([.. artistsByKey.Values])
        };
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
