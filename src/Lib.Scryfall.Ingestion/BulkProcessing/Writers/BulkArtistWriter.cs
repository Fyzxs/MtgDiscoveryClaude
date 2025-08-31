using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.Dtos;
using Lib.Adapter.Scryfall.Cosmos.Apis.Entities;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators;
using Lib.Scryfall.Ingestion.BulkProcessing.Storage;
using Microsoft.Extensions.Logging;

namespace Lib.Scryfall.Ingestion.BulkProcessing.Writers;

internal sealed class BulkArtistWriter : IBulkArtistWriter
{
    private readonly ScryfallArtistItemsScribe _artistScribe;

    public BulkArtistWriter(ILogger logger)
    {
        _artistScribe = new ScryfallArtistItemsScribe(logger);
    }

    public async Task WriteArtistsAsync(IEnumerable<BulkArtistData> artists)
    {
        List<Task> writeTasks = [];

        foreach (BulkArtistData artist in artists)
        {
            ArtistAggregateData aggregateData = new()
            {
                ArtistId = artist.ArtistId,
                ArtistNames = artist.ArtistNames,
                ArtistNamesSearch = string.Join(" ", artist.ArtistNames).ToLowerInvariant(),
                CardIds = artist.CardIds,
                SetIds = artist.SetIds
            };

            ScryfallArtistItem artistItem = new(artist.ArtistId, aggregateData);

            writeTasks.Add(_artistScribe.UpsertAsync(artistItem));
        }

        await Task.WhenAll(writeTasks).ConfigureAwait(false);
    }
}