using Lib.Scryfall.Ingestion.Apis.Models;
using Lib.Scryfall.Ingestion.Cosmos.Processors;
using Microsoft.Extensions.Logging;

namespace Example.Scryfall.CosmosIngestion;

internal interface ICardProcessor
{
    Task ProcessCardAsync(IScryfallCard card);
}

internal sealed class CardProcessor : ICardProcessor
{
    private readonly ICardsProcessor _cardsProcessor;
    private readonly ILogger _logger;

    public CardProcessor(ILogger logger)
        : this(
            new CardsProcessor(logger),
            logger)
    {
    }

    private CardProcessor(
        ICardsProcessor cardsProcessor,
        ILogger logger)
    {
        _cardsProcessor = cardsProcessor;
        _logger = logger;
    }

    public async Task ProcessCardAsync(IScryfallCard card)
    {
        try
        {
            _logger.LogProcessingCard(card.Name());
            await _cardsProcessor.ProcessAsync(card).ConfigureAwait(false);
        }
        catch (Exception ex) when (ex is TaskCanceledException or HttpRequestException or InvalidOperationException)
        {
            _logger.LogCardProcessingError(ex, card.Name());
            throw;
        }
    }
}

internal static partial class CardProcessorOrchestratorLoggerExtensions
{
    [LoggerMessage(
        Level = LogLevel.Debug,
        Message = "Processing card: {Name}")]
    public static partial void LogProcessingCard(this ILogger logger, string name);

    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "Error processing card {Name}")]
    public static partial void LogCardProcessingError(this ILogger logger, Exception ex, string name);
}
