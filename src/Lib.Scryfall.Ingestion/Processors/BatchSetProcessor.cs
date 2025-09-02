using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Lib.Scryfall.Ingestion.Configurations;
using Lib.Scryfall.Shared.Apis.Models;
using Microsoft.Extensions.Logging;

namespace Lib.Scryfall.Ingestion.Processors;

internal sealed class BatchSetProcessor : IBatchSetProcessor
{
    private readonly ISetProcessor _setProcessor;
    private readonly ArtistAggregationWriterProcessor _artistWriter;
    private readonly CardNameTrigramWriterProcessor _trigramWriter;
    private readonly IScryfallIngestionConfiguration _config;
    private readonly ILogger _logger;

    public BatchSetProcessor(ILogger logger)
        : this(
            CreateSetProcessor(logger),
            new ArtistAggregationWriterProcessor(logger),
            new CardNameTrigramWriterProcessor(logger),
            new ConfigScryfallIngestionConfiguration(),
            logger)
    {
    }

    private static ISetProcessor CreateSetProcessor(ILogger logger)
    {
        IScryfallIngestionConfiguration config = new ConfigScryfallIngestionConfiguration();
        bool processOnlySetItems = config.ProcessingConfig().ProcessOnlySetItems().AsSystemType();

        if (processOnlySetItems)
        {
            logger.LogSetItemsOnlyModeEnabled();
            return new SetItemsOnlyProcessor(logger);
        }

        logger.LogFullProcessingModeEnabled();
        return new SetProcessor(logger);
    }

    private BatchSetProcessor(
        ISetProcessor setProcessor,
        ArtistAggregationWriterProcessor artistWriter,
        CardNameTrigramWriterProcessor trigramWriter,
        IScryfallIngestionConfiguration config,
        ILogger logger)
    {
        _setProcessor = setProcessor;
        _artistWriter = artistWriter;
        _trigramWriter = trigramWriter;
        _config = config;
        _logger = logger;
    }

    public async Task ProcessSetsAsync(IEnumerable<IScryfallSet> sets)
    {
        List<IScryfallSet> setList = sets.ToList();
        int totalSets = setList.Count;
        int processedSets = 0;

        _logger.LogProcessingStart(totalSets);

        // Process all sets
        foreach (IScryfallSet set in setList)
        {
            try
            {
                await _setProcessor.ProcessAsync(set).ConfigureAwait(false);
                processedSets++;
                _logger.LogSetProcessed(processedSets, totalSets);
            }
            catch (Exception ex) when (ex is TaskCanceledException or HttpRequestException or InvalidOperationException)
            {
                _logger.LogBatchSetProcessingError(ex, set.Code());
            }
        }

        // Write all aggregated data at the end
        _logger.LogWritingAggregatedData();

        // Write dirty artists after all sets are processed
        await _artistWriter.ProcessAsync().ConfigureAwait(false);

        // Write card name trigrams after all sets are processed
        await _trigramWriter.WriteTrigramsAsync().ConfigureAwait(false);

        _logger.LogAllSetsComplete(totalSets);
    }
}

internal static partial class BatchSetProcessorLoggerExtensions
{
    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Configuration: Processing ONLY set items (skipping cards)")]
    public static partial void LogSetItemsOnlyModeEnabled(this ILogger logger);

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Configuration: Processing full sets including cards")]
    public static partial void LogFullProcessingModeEnabled(this ILogger logger);
    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Starting to process {TotalSets} sets")]
    public static partial void LogProcessingStart(this ILogger logger, int totalSets);

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Processed {ProcessedSets}/{TotalSets} sets")]
    public static partial void LogSetProcessed(this ILogger logger, int processedSets, int totalSets);

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Writing aggregated data to Cosmos DB")]
    public static partial void LogWritingAggregatedData(this ILogger logger);

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "All sets complete. Total sets processed: {TotalSets}")]
    public static partial void LogAllSetsComplete(this ILogger logger, int totalSets);

    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "Error processing set {Code}")]
    public static partial void LogBatchSetProcessingError(this ILogger logger, Exception ex, string code);
}
