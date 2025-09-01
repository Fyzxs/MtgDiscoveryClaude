using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.Entities;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators;
using Lib.Scryfall.Ingestion.Apis.Configuration;
using Lib.Scryfall.Ingestion.Apis.Dashboard;
using Lib.Scryfall.Ingestion.Apis.Pipeline;
using Lib.Scryfall.Ingestion.BulkIngestion;
using Microsoft.Extensions.Logging;

namespace Lib.Scryfall.Ingestion.Pipeline;

internal sealed class CardsPipelineService : ICardsPipelineService
{
    private readonly CardsBulkDataFetcher _cardsFetcher;
    private readonly ScryfallCardItemsScribe _cardScribe;
    private readonly IIngestionDashboard _dashboard;
    private readonly IBulkProcessingConfiguration _config;

    public CardsPipelineService(
        CardsBulkDataFetcher cardsFetcher,
        ScryfallCardItemsScribe cardScribe,
        IIngestionDashboard dashboard,
        IBulkProcessingConfiguration config)
    {
        _cardsFetcher = cardsFetcher;
        _cardScribe = cardScribe;
        _dashboard = dashboard;
        _config = config;
    }

    public async Task<IReadOnlyList<dynamic>> FetchCardsAsync()
    {
        _dashboard.LogFetchingCards();

        List<dynamic> cards = [];
        IAsyncEnumerable<dynamic> cardStream = await _cardsFetcher.FetchCardsAsync().ConfigureAwait(false);
        int totalCards = 0;

        await foreach (dynamic card in cardStream.ConfigureAwait(false))
        {
            cards.Add(card);
            totalCards++;

            string cardName = card.name;
            string setCode = card.set;
            _dashboard.UpdateCardProgress(totalCards, 0, $"Fetching: {cardName} [{setCode}]");

            if (_config.EnableMemoryThrottling)
            {
                _dashboard.UpdateMemoryUsage();
            }

            if (totalCards % _config.DashboardRefreshFrequency == 0)
            {
                _dashboard.Refresh();
            }
        }

        _dashboard.LogCardsFetched(totalCards);
        return cards;
    }

    public void ProcessCards(IReadOnlyList<dynamic> cards, IReadOnlyDictionary<string, dynamic> setsByCode)
    {
        _dashboard.LogProcessingCards(cards.Count);

        // Processing logic will be added here
        // - Extract artists
        // - Build set-card relationships
        // - Other processing
    }

    public async Task WriteCardsAsync(IReadOnlyList<dynamic> cards)
    {
        _dashboard.LogWritingCards(cards.Count);

        int current = 0;
        int total = cards.Count;

        foreach (dynamic card in cards)
        {
            current++;
            string cardName = card.name;
            string setCode = card.set;
            _dashboard.UpdateCardProgress(current, total, $"Writing: [{setCode}] {cardName}");

            if (current % _config.DashboardRefreshFrequency == 0)
            {
                _dashboard.Refresh();
            }

            ScryfallCardItem entity = new()
            {
                Data = card
            };

            await _cardScribe.UpsertAsync(entity).ConfigureAwait(false);
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
        Message = "Writing {Count} cards to Cosmos DB")]
    public static partial void LogWritingCards(this ILogger logger, int count);

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Successfully wrote {Count} cards to Cosmos DB")]
    public static partial void LogCardsWritten(this ILogger logger, int count);
}
