using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Lib.Scryfall.Shared.Apis.Models;

namespace Lib.Scryfall.Ingestion.Processors;

internal sealed class SetProcessor : ISetProcessor
{
    private readonly IReadOnlyCollection<ISetProcessor> _setItemsProcessor;
    private readonly ICardProcessor _cardProcessor;
    private readonly ILogger _logger;

    public SetProcessor(ILogger logger)
        : this(
            [
                new SetItemsProcessor(logger),
                new SetAssociationsProcessor(logger),
                new SetCodeIndexProcessor(logger)
            ],
            new CardProcessor(logger),
            logger)
    {
    }
    private SetProcessor(IReadOnlyCollection<ISetProcessor> setItemsProcessor,
        ICardProcessor cardProcessor,
        ILogger logger)
    {
        _setItemsProcessor = setItemsProcessor;
        _cardProcessor = cardProcessor;
        _logger = logger;
    }

    public async Task ProcessAsync(IScryfallSet set)
    {
        try
        {
            _logger.LogProcessingSet(set.Code(), set.Name());

            foreach (ISetProcessor setProcessor in _setItemsProcessor)
            {
                await setProcessor.ProcessAsync(set).ConfigureAwait(false);
            }

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
                await _cardProcessor.ProcessAsync(card).ConfigureAwait(false);
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

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Error processing card {name}")]
    public static partial void LogCardProcessingError(this ILogger logger, Exception ex, string name);
}
