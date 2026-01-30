using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Scryfall.Shared.Apis.Models;
using Microsoft.Extensions.Logging;

namespace Lib.Scryfall.Ingestion.Processors;

internal sealed class CardProcessor : ICardProcessor
{
    private readonly IReadOnlyList<ICardProcessor> _processors;
    private readonly ILogger _logger;

    public CardProcessor(ILogger logger)
        : this(
            [
                new SetCardsProcessor(logger),
                new CardItemsProcessor(logger),
                new ArtistAggregatorProcessor(logger),
                new CardsByNameProcessor(logger),
                new CardNameTrigramAggregatorProcessor(logger)
            ],
            logger)
    {
    }

    private CardProcessor(IReadOnlyList<ICardProcessor> processors, ILogger logger)
    {
        _processors = processors;
        _logger = logger;
    }

    public async Task ProcessAsync(IScryfallCard card)
    {
        string cardId = card.Id();
        _logger.LogCardProcessingStarted(card.Name(), cardId);

        foreach (ICardProcessor processor in _processors)
        {
            await processor.ProcessAsync(card).ConfigureAwait(false);
        }

        _logger.LogCardProcessingCompleted(card.Name(), cardId);
    }
}

internal static partial class CardsProcessorLoggerExtensions
{
    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Processing card '{CardName}' ({CardId})")]
    public static partial void LogCardProcessingStarted(this ILogger logger, string cardName, string cardId);

    [LoggerMessage(
        Level = LogLevel.Debug,
        Message = "Completed processing card '{CardName}' ({CardId})")]
    public static partial void LogCardProcessingCompleted(this ILogger logger, string cardName, string cardId);
}
