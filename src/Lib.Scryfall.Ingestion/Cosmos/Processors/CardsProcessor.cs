using System.Threading.Tasks;
using Lib.Scryfall.Ingestion.Apis.Models;
using Microsoft.Extensions.Logging;

namespace Lib.Scryfall.Ingestion.Cosmos.Processors;

internal sealed class CardsProcessor : ICardsProcessor
{
    private readonly ICardsProcessor _setCardsProcessor;
    private readonly ICardsProcessor _cardItemsProcessor;

    public CardsProcessor(ILogger logger)
        : this(
            new SetCardsProcessor(logger),
            new CardItemsProcessor(logger))
    {
    }

    private CardsProcessor(ICardsProcessor setCardsProcessor, ICardsProcessor cardItemsProcessor)
    {
        _setCardsProcessor = setCardsProcessor;
        _cardItemsProcessor = cardItemsProcessor;
    }

    public async Task ProcessAsync(IScryfallCard card)
    {
        await _setCardsProcessor.ProcessAsync(card).ConfigureAwait(false);
        await _cardItemsProcessor.ProcessAsync(card).ConfigureAwait(false);
    }
}