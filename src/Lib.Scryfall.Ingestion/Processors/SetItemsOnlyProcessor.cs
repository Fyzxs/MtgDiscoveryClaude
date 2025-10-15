using System.Threading.Tasks;
using Lib.Scryfall.Shared.Apis.Models;

namespace Lib.Scryfall.Ingestion.Processors;

internal sealed class SetItemsOnlyProcessor : ISetProcessor
{
    private readonly ISetProcessor _setItemsProcessor;
    private readonly ILogger _logger;

    public SetItemsOnlyProcessor(ILogger logger)
        : this(new SetItemsProcessor(logger), logger)
    {
    }

    private SetItemsOnlyProcessor(ISetProcessor setItemsProcessor, ILogger logger)
    {
        _setItemsProcessor = setItemsProcessor;
        _logger = logger;
    }

    public async Task ProcessAsync(IScryfallSet set)
    {
        _logger.LogProcessingSetItemsOnly(set.Code(), set.Name());
        await _setItemsProcessor.ProcessAsync(set).ConfigureAwait(false);
        _logger.LogSetItemsOnlyProcessed(set.Code());
    }
}

internal static partial class SetItemsOnlyProcessorLoggerExtensions
{
    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Processing ONLY set items for: {Code} - {Name}")]
    public static partial void LogProcessingSetItemsOnly(this ILogger logger, string code, string name);

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Completed processing set items only for: {Code}")]
    public static partial void LogSetItemsOnlyProcessed(this ILogger logger, string code);
}
