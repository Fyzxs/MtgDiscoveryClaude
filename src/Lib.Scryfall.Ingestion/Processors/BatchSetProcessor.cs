using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.Scryfall.Ingestion.Configurations;
using Lib.Scryfall.Shared.Apis.Models;
using Microsoft.Extensions.Logging;

namespace Lib.Scryfall.Ingestion.Processors;

internal sealed class BatchSetProcessor
{
    private readonly ISetProcessor _setProcessor;
    private readonly ArtistAggregationWriterProcessor _artistWriter;
    private readonly SetBatchSize _batchSizeConfig;
    private readonly ProcessSetsInReverse _processSetsInReverseConfig;
    private readonly ILogger _logger;

    public BatchSetProcessor(ILogger logger)
        : this(
            new SetProcessor(logger),
            new ArtistAggregationWriterProcessor(logger),
            new ConfigScryfallIngestionConfiguration().ProcessingConfig().SetBatchSize(),
            new ConfigScryfallIngestionConfiguration().ProcessingConfig().ProcessSetsInReverse(),
            logger)
    {
    }

    private BatchSetProcessor(
        ISetProcessor setProcessor,
        ArtistAggregationWriterProcessor artistWriter,
        SetBatchSize batchSizeConfig,
        ProcessSetsInReverse processSetsInReverseConfig,
        ILogger logger)
    {
        _setProcessor = setProcessor;
        _artistWriter = artistWriter;
        _batchSizeConfig = batchSizeConfig;
        _processSetsInReverseConfig = processSetsInReverseConfig;
        _logger = logger;
    }

    public async Task ProcessSetsAsync(IEnumerable<IScryfallSet> sets)
    {
        int batchSize = _batchSizeConfig.AsSystemType();
        bool processInReverse = _processSetsInReverseConfig.AsSystemType();

        List<IScryfallSet> setList = sets.ToList();
        if (processInReverse)
        {
            setList.Reverse();
            _logger.LogProcessingInReverse();
        }

        int totalSets = setList.Count;
        int processedSets = 0;

        while (processedSets < totalSets)
        {
            List<IScryfallSet> batch = setList.Skip(processedSets).Take(batchSize).ToList();

            _logger.LogBatchStart(processedSets, batch.Count, totalSets);

            // Process the batch of sets
            foreach (IScryfallSet set in batch)
            {
                await _setProcessor.ProcessAsync(set).ConfigureAwait(false);
            }

            // Write dirty artists after each batch
            await _artistWriter.ProcessAsync().ConfigureAwait(false);

            processedSets += batch.Count;
            _logger.LogBatchComplete(processedSets, totalSets);
        }

        _logger.LogAllBatchesComplete(totalSets);
    }
}

internal static partial class BatchSetProcessorLoggerExtensions
{
    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Starting batch: {ProcessedSets}/{TotalSets} sets, batch size: {BatchSize}")]
    public static partial void LogBatchStart(this ILogger logger, int processedSets, int batchSize, int totalSets);

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Batch complete: {ProcessedSets}/{TotalSets} sets processed")]
    public static partial void LogBatchComplete(this ILogger logger, int processedSets, int totalSets);

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "All batches complete: {TotalSets} sets processed")]
    public static partial void LogAllBatchesComplete(this ILogger logger, int totalSets);

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Processing sets in reverse order (oldest first)")]
    public static partial void LogProcessingInReverse(this ILogger logger);
}
