using Lib.Scryfall.Ingestion.Apis.Collections;
using Lib.Scryfall.Ingestion.Apis.Models;
using Lib.Scryfall.Ingestion.Apis.Processors;
using Microsoft.Extensions.Logging;

namespace Example.Scryfall.CosmosIngestion;

internal sealed class ScryfallIngestionService : IScryfallIngestionService
{
    private readonly IAsyncEnumerable<IScryfallSet> _scryfallSets;
    private readonly ISetProcessor _setProcessor;
    private readonly ILogger _logger;

    public ScryfallIngestionService(ILogger logger)
        : this(
            logger,
            new FilteredScryfallSetCollection(logger),
            new SetProcessor(logger))
    {
    }

    private ScryfallIngestionService(
        ILogger logger,
        IAsyncEnumerable<IScryfallSet> scryfallSets,
        ISetProcessor setProcessor)
    {
        _logger = logger;
        _scryfallSets = scryfallSets;
        _setProcessor = setProcessor;
    }

    public async Task IngestAllSetsAsync()
    {
        int processedCount = 0;

        _logger.LogInformation("Starting Scryfall sets ingestion to Cosmos DB (excluding digital sets)");

        await foreach (IScryfallSet set in _scryfallSets.ConfigureAwait(false))
        {
            try
            {
                await _setProcessor.ProcessAsync(set).ConfigureAwait(false);
                processedCount++;
            }
            catch (Exception ex) when (ex is TaskCanceledException or HttpRequestException or InvalidOperationException)
            {
                _logger.LogError(ex, "Error processing set {Code}", set.Code());
            }
        }

        _logger.LogInformation("Ingestion complete. Processed {Count} sets", processedCount);
    }
}
