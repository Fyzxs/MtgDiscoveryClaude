using Lib.Scryfall.Ingestion.Apis.Models;
using Lib.Scryfall.Ingestion.Cosmos.Processors;
using Lib.Scryfall.Ingestion.Icons.Processors;
using Microsoft.Extensions.Logging;

namespace Example.Scryfall.CosmosIngestion;

internal interface ISetProcessorOrchestrator
{
    Task ProcessSetAsync(IScryfallSet set);
}

internal sealed class SetProcessor : ISetProcessorOrchestrator
{
    private readonly ISetItemsProcessor _setItemsProcessor;
    private readonly ISetAssociationsProcessor _setAssociationsProcessor;
    private readonly ISetIconProcessor _setIconProcessor;
    private readonly ILogger _logger;

    public SetProcessor(ILogger logger)
        : this(
            new SetItemsProcessor(logger),
            new SetAssociationsProcessor(logger),
            new SetIconProcessor(logger),
            logger)
    {
    }

    private SetProcessor(
        ISetItemsProcessor setItemsProcessor,
        ISetAssociationsProcessor setAssociationsProcessor,
        ISetIconProcessor setIconProcessor,
        ILogger logger)
    {
        _setItemsProcessor = setItemsProcessor;
        _setAssociationsProcessor = setAssociationsProcessor;
        _setIconProcessor = setIconProcessor;
        _logger = logger;
    }

    public async Task ProcessSetAsync(IScryfallSet set)
    {
        try
        {
            _logger.LogProcessingSet(set.Code(), set.Name());

            await _setItemsProcessor.ProcessAsync(set).ConfigureAwait(false);
            await _setAssociationsProcessor.ProcessAsync(set).ConfigureAwait(false);
            await _setIconProcessor.ProcessAsync(set).ConfigureAwait(false);
        }
        catch (Exception ex) when (ex is TaskCanceledException or HttpRequestException or InvalidOperationException)
        {
            _logger.LogSetProcessingError(ex, set.Code());
            throw;
        }
    }
}

internal static partial class SetProcessorOrchestratorLoggerExtensions
{
    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Processing set: {Code} - {Name}")]
    public static partial void LogProcessingSet(this ILogger logger, string code, string name);

    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "Error processing set {Code}")]
    public static partial void LogSetProcessingError(this ILogger logger, Exception ex, string code);
}
