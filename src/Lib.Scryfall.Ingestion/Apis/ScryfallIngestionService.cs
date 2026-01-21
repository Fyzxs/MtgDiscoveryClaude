using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Scryfall.Ingestion.Apis.Collections;
using Lib.Scryfall.Ingestion.Processors;
using Lib.Scryfall.Shared.Apis.Models;

namespace Lib.Scryfall.Ingestion.Apis;

internal sealed class ScryfallIngestionService : IScryfallIngestionService
{
    private readonly IAsyncEnumerable<IScryfallSet> _scryfallSets;
    private readonly IBatchSetProcessor _batchSetProcessor;
    private readonly ILogger _logger;

    public ScryfallIngestionService(ILogger logger)
        : this(
            logger,
            new FilteredScryfallSetCollection(logger),
            new BatchSetProcessor(logger))
    {
    }

    private ScryfallIngestionService(
        ILogger logger,
        IAsyncEnumerable<IScryfallSet> scryfallSets,
        IBatchSetProcessor batchSetProcessor)
    {
        _logger = logger;
        _scryfallSets = scryfallSets;
        _batchSetProcessor = batchSetProcessor;
    }

    public async Task IngestAllSetsAsync()
    {
        _logger.LogIngestionStarted();

        // Collect all sets into a list for batch processing
        List<IScryfallSet> allSets = [];
        await foreach (IScryfallSet set in _scryfallSets.ConfigureAwait(false))
        {
            allSets.Add(set);
        }

        _logger.LogSetsCollected(allSets.Count);

        // Process all sets in batches
        // BatchSetProcessor handles:
        // - Batch size configuration
        // - Reverse order processing if configured
        // - Artist aggregation updates after each batch
        // - Individual set error handling
        await _batchSetProcessor.ProcessSetsAsync(allSets).ConfigureAwait(false);

        _logger.LogIngestionCompleted(allSets.Count);
    }
}

internal static partial class ScryfallIngestionServiceLoggerExtensions
{
    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Starting Scryfall sets ingestion to Cosmos DB (excluding digital sets)")]
    public static partial void LogIngestionStarted(this ILogger logger);

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Collected {SetCount} sets for processing")]
    public static partial void LogSetsCollected(this ILogger logger, int setCount);

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Ingestion complete. Processed {ProcessedCount} sets")]
    public static partial void LogIngestionCompleted(this ILogger logger, int processedCount);
}
