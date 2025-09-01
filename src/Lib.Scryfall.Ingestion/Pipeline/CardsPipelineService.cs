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

    public async Task<int> ProcessCardsAsync()
    {
        int totalCards = await FetchAndWriteCardsAsync().ConfigureAwait(false);
        return totalCards;
    }

    private async Task<int> FetchAndWriteCardsAsync()
    {
        _dashboard.LogFetchingCards();

        IAsyncEnumerable<dynamic> cardStream = await _cardsFetcher.FetchCardsAsync().ConfigureAwait(false);
        int totalCards = 0;

        await foreach (dynamic card in cardStream.ConfigureAwait(false))
        {
            totalCards++;

            string cardName = card.name;
            _dashboard.UpdateCardProgress(totalCards, 0, $"Processing: {cardName}");

            if (_config.EnableMemoryThrottling)
            {
                _dashboard.UpdateMemoryUsage();
            }

            if (totalCards % _config.DashboardRefreshFrequency == 0)
            {
                _dashboard.Refresh();
            }

            // Write the card directly to Cosmos
            ScryfallCardItem entity = new()
            {
                Data = card
            };

            await _cardScribe.UpsertAsync(entity).ConfigureAwait(false);
        }

        _dashboard.LogCardsProcessed(totalCards);
        return totalCards;
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
        Message = "Processed {Count} cards")]
    public static partial void LogCardsProcessed(this ILogger logger, int count);
}
