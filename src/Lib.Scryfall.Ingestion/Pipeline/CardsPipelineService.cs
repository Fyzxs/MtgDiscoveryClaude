using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.Entities;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators;
using Lib.Scryfall.Ingestion.Apis.Configuration;
using Lib.Scryfall.Ingestion.Apis.Dashboard;
using Lib.Scryfall.Ingestion.Apis.Pipeline;
using Lib.Scryfall.Ingestion.BulkIngestion;
using Lib.Scryfall.Ingestion.Mappers;
using Lib.Scryfall.Shared.Apis.Models;
using Microsoft.Extensions.Logging;

namespace Lib.Scryfall.Ingestion.Pipeline;

internal sealed class CardsPipelineService : ICardsPipelineService
{
    private readonly CardsBulkDataFetcher _cardsFetcher;
    private readonly ScryfallCardItemsScribe _cardScribe;
    private readonly ScryfallSetCardsScribe _setCardsScribe;
    private readonly IScryfallCardToSetCardMapper _setCardMapper;
    private readonly IIngestionDashboard _dashboard;
    private readonly IBulkProcessingConfiguration _config;

    public CardsPipelineService(
        CardsBulkDataFetcher cardsFetcher,
        ScryfallCardItemsScribe cardScribe,
        IIngestionDashboard dashboard,
        IBulkProcessingConfiguration config,
        ILogger logger)
    {
        _cardsFetcher = cardsFetcher;
        _cardScribe = cardScribe;
        _setCardsScribe = new ScryfallSetCardsScribe(logger);
        _setCardMapper = new ScryfallCardToSetCardMapper();
        _dashboard = dashboard;
        _config = config;
    }

    public async Task<IReadOnlyList<IScryfallCard>> FetchCardsAsync()
    {
        _dashboard.LogFetchingCards();

        List<IScryfallCard> cards = [];
        IAsyncEnumerable<IScryfallCard> cardStream = await _cardsFetcher.FetchCardsAsync().ConfigureAwait(false);
        int totalCards = 0;
        int skippedCards = 0;
        bool hasSetFilter = _config.SetCodesToProcess.Count > 0;

        await foreach (IScryfallCard card in cardStream.ConfigureAwait(false))
        {
            string setCode = card.Set().Code();

            // Skip cards from sets not in the filter list if filtering is enabled
            if (hasSetFilter && !_config.SetCodesToProcess.Contains(setCode))
            {
                skippedCards++;
                continue;
            }

            cards.Add(card);
            totalCards++;

            string cardName = card.Name();
            _dashboard.UpdateProgress("Cards:", totalCards, 0, "Fetching", $"[{setCode}] {cardName}");
        }

        if (skippedCards > 0)
        {
            _dashboard.LogCardsSkipped(skippedCards);
        }

        _dashboard.LogCardsFetched(totalCards);
        return cards;
    }

    public void ProcessCards(IReadOnlyList<IScryfallCard> cards, IReadOnlyDictionary<string, dynamic> setsByCode)
    {
        _dashboard.LogProcessingCards(cards.Count);
        // Card processing logic (if any) goes here
    }

    public async Task WriteCardsAsync(IReadOnlyList<IScryfallCard> cards)
    {
        _dashboard.LogWritingCards(cards.Count);

        int current = 0;
        int total = cards.Count;

        foreach (IScryfallCard card in cards)
        {
            current++;
            string cardName = card.Name();
            string setCode = card.Set().Code();
            _dashboard.UpdateProgress("Cards:", current, total, "Writing", $"[{setCode}] {cardName}");

            // Write the card item
            ScryfallCardItem cardItem = new()
            {
                Data = card.Data()
            };
            await _cardScribe.UpsertAsync(cardItem).ConfigureAwait(false);

            // Write the set-card relationship
            ScryfallSetCard setCard = _setCardMapper.Map(card.Data());
            await _setCardsScribe.UpsertAsync(setCard).ConfigureAwait(false);
        }

        _dashboard.LogCardsWritten(cards.Count);
    }

}

internal static partial class CardsPipelineServiceLoggerExtensions
{
    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Fetching cards from Scryfall bulk data")]
    public static partial void LogFetchingCards(this ILogger logger);

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Fetched {Count} cards")]
    public static partial void LogCardsFetched(this ILogger logger, int count);

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Processing {Count} cards")]
    public static partial void LogProcessingCards(this ILogger logger, int count);

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Writing {Count} cards and set-card relationships to Cosmos DB")]
    public static partial void LogWritingCards(this ILogger logger, int count);

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Successfully wrote {Count} cards and set-card relationships to Cosmos DB")]
    public static partial void LogCardsWritten(this ILogger logger, int count);

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Skipped {Count} cards due to set filter configuration")]
    public static partial void LogCardsSkipped(this ILogger logger, int count);
}
