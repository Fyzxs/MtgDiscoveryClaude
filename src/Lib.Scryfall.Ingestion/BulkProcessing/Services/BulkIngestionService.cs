using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Lib.Scryfall.Ingestion.Apis.Collections;
using Lib.Scryfall.Ingestion.BulkProcessing.Fetchers;
using Lib.Scryfall.Ingestion.BulkProcessing.Loaders;
using Lib.Scryfall.Ingestion.BulkProcessing.Models;
using Lib.Scryfall.Ingestion.BulkProcessing.Storage;
using Lib.Scryfall.Ingestion.BulkProcessing.Writers;
using Microsoft.Extensions.Logging;

namespace Lib.Scryfall.Ingestion.BulkProcessing.Services;

internal sealed class BulkIngestionService : IBulkIngestionService
{
    private readonly ILogger _logger;
    private readonly IBulkDataMetadataFetcher _metadataFetcher;
    private readonly IBulkDataDownloader _downloader;
    private readonly IBulkSetLoader _setLoader;
    private readonly IBulkCardLoader _cardLoader;
    private readonly IBulkRulingLoader _rulingLoader;
    private readonly IBulkArtistWriter _artistWriter;
    private readonly IBulkRulingWriter _rulingWriter;

    public BulkIngestionService(ILogger logger)
    {
        _logger = logger;
        _metadataFetcher = new BulkDataMetadataFetcher();
        _downloader = new BulkDataDownloader();
        _setLoader = new BulkSetLoader();
        _cardLoader = new BulkCardLoader();
        _rulingLoader = new BulkRulingLoader();
        _artistWriter = new BulkArtistWriter(logger);
        _rulingWriter = new BulkRulingWriter(logger);
    }

    public async Task IngestBulkDataAsync()
    {
        // Initialize storage
        BulkSetStorage setStorage = new();
        BulkArtistStorage artistStorage = new();
        BulkRulingStorage rulingStorage = new();

        // Step 1: Load filtered sets
        FilteredScryfallSetCollection setsCollection = new(_logger);
        await _setLoader.LoadSetsAsync(setsCollection, setStorage).ConfigureAwait(false);

        // Step 2: Fetch and load rulings
        IScryfallBulkMetadata rulingsMetadata = await _metadataFetcher.FetchRulingsMetadataAsync().ConfigureAwait(false);
        if (rulingsMetadata != null)
        {
            using Stream rulingsStream = await _downloader.DownloadAsync(rulingsMetadata).ConfigureAwait(false);
            await _rulingLoader.LoadRulingsAsync(rulingsStream, rulingStorage).ConfigureAwait(false);
        }

        // Step 3: Fetch and load cards
        IScryfallBulkMetadata cardsMetadata = await _metadataFetcher.FetchAllCardsMetadataAsync().ConfigureAwait(false);
        if (cardsMetadata != null)
        {
            using Stream cardsStream = await _downloader.DownloadAsync(cardsMetadata).ConfigureAwait(false);
            await _cardLoader.LoadCardsAsync(cardsStream, setStorage, artistStorage).ConfigureAwait(false);
        }

        // Step 4: Write to Cosmos
        await _artistWriter.WriteArtistsAsync(artistStorage.GetAll()).ConfigureAwait(false);
        await _rulingWriter.WriteRulingsAsync(rulingStorage.GetAll()).ConfigureAwait(false);
    }
}