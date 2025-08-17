using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Lib.Scryfall.Ingestion.Apis.Collections;
using Lib.Scryfall.Ingestion.Processors;
using Lib.Scryfall.Shared.Apis.Models;
using Microsoft.Extensions.Logging;

namespace Lib.Scryfall.Ingestion.Apis;

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

        _logger.LogIngestionStarted();

        await foreach (IScryfallSet set in _scryfallSets.ConfigureAwait(false))
        {
            try
            {
                await _setProcessor.ProcessAsync(set).ConfigureAwait(false);
                processedCount++;
            }
            catch (Exception ex) when (ex is TaskCanceledException or HttpRequestException or InvalidOperationException)
            {
                _logger.LogSetProcessingError(ex, set.Code());
            }
        }

        _logger.LogIngestionCompleted(processedCount);
    }
}

internal static partial class ScryfallIngestionServiceLoggerExtensions
{
    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Starting Scryfall sets ingestion to Cosmos DB (excluding digital sets)")]
    public static partial void LogIngestionStarted(this ILogger logger);

    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "Error processing set {SetCode}")]
    public static partial void LogSetProcessingError(this ILogger logger, Exception ex, string setCode);

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Ingestion complete. Processed {ProcessedCount} sets")]
    public static partial void LogIngestionCompleted(this ILogger logger, int processedCount);
}
