using Lib.Scryfall.Ingestion.Apis.Models;
using Lib.Scryfall.Ingestion.Processors;
using Microsoft.Extensions.Logging;

namespace Example.Scryfall.CosmosIngestion;

internal sealed class SetProcessor : ISetProcessor
{
    private readonly ISetProcessor _setItemsProcessor;
    private readonly ISetProcessor _setAssociationsProcessor;
    private readonly ISetProcessor _setIconProcessor;
    private readonly ICardProcessor _cardProcessor;
    private readonly ILogger _logger;

    public SetProcessor(ILogger logger)
        : this(
            new SetItemsProcessor(logger),
            new SetAssociationsProcessor(logger),
            new SetIconProcessor(logger),
            new CardProcessor(logger),
            logger)
    {
    }

    private SetProcessor(
        ISetProcessor setItemsProcessor,
        ISetProcessor setAssociationsProcessor,
        ISetProcessor setIconProcessor,
        ICardProcessor cardProcessor,
        ILogger logger)
    {
        _setItemsProcessor = setItemsProcessor;
        _setAssociationsProcessor = setAssociationsProcessor;
        _setIconProcessor = setIconProcessor;
        _cardProcessor = cardProcessor;
        _logger = logger;
    }

    public async Task ProcessAsync(IScryfallSet set)
    {
        try
        {
            _logger.LogProcessingSet(set.Code(), set.Name());

            await _setItemsProcessor.ProcessAsync(set).ConfigureAwait(false);
            await _setAssociationsProcessor.ProcessAsync(set).ConfigureAwait(false);
            await _setIconProcessor.ProcessAsync(set).ConfigureAwait(false);

            await ProcessSetCardsAsync(set).ConfigureAwait(false);
        }
        catch (Exception ex) when (ex is TaskCanceledException or HttpRequestException or InvalidOperationException)
        {
            _logger.LogSetProcessingError(ex, set.Code());
            throw;
        }
    }

    private async Task ProcessSetCardsAsync(IScryfallSet set)
    {
        int cardCount = 0;
        _logger.LogProcessingSetCards(set.Code());

        await foreach (IScryfallCard card in set.Cards().ConfigureAwait(false))
        {
            try
            {
                await _cardProcessor.ProcessCardAsync(card).ConfigureAwait(false);
                cardCount++;
            }
            catch (Exception ex) when (ex is TaskCanceledException or HttpRequestException or InvalidOperationException)
            {
                _logger.LogCardProcessingError(ex, card.Name());
            }
        }

        _logger.LogSetCardsProcessed(set.Code(), cardCount);
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

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Processing cards for set {Code}")]
    public static partial void LogProcessingSetCards(this ILogger logger, string code);

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Processed {Count} cards for set {Code}")]
    public static partial void LogSetCardsProcessed(this ILogger logger, string code, int count);
}
