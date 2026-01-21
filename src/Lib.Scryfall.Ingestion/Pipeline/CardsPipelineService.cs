using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Scribes;
using Lib.Scryfall.Ingestion.Apis.Configuration;
using Lib.Scryfall.Ingestion.Apis.Dashboard;
using Lib.Scryfall.Ingestion.Apis.Pipeline;
using Lib.Scryfall.Ingestion.BulkIngestion;
using Lib.Scryfall.Ingestion.Mappers;
using Lib.Scryfall.Shared.Apis.Models;
using Lib.Shared.Abstractions.Identifiers;
using Microsoft.Extensions.Logging;

namespace Lib.Scryfall.Ingestion.Pipeline;

internal sealed class CardsPipelineService : ICardsPipelineService
{
    private readonly CardsBulkDataFetcher _cardsFetcher;
    private readonly ScryfallCardItemsScribe _cardScribe;
    private readonly ScryfallCardsByNameScribe _cardsByNameScribe;
    private readonly ScryfallSetCardsScribe _setCardsScribe;
    private readonly ISetCardItemDynamicToExtMapper _setCardMapper;
    private readonly ICardNameGuidGenerator _guidGenerator;
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
        _cardsByNameScribe = new ScryfallCardsByNameScribe(logger);
        _setCardsScribe = new ScryfallSetCardsScribe(logger);
        _setCardMapper = new SetCardItemDynamicToExtMapper();
        _guidGenerator = new CardNameGuidGenerator();
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
        DateTime? releasedAfter = _config.SetsReleasedAfter;

        await foreach (IScryfallCard card in cardStream.ConfigureAwait(false))
        {
            string setCode = card.Set().Code();

            // Skip cards from sets not in the filter list if filtering is enabled
            if (hasSetFilter && _config.SetCodesToProcess.Contains(setCode) is false)
            {
                skippedCards++;
                continue;
            }

            // Skip cards from sets released before the date filter if enabled
            if (releasedAfter.HasValue)
            {
                DateTime setReleaseDate = GetSetReleaseDate(card.Set());
                if (setReleaseDate < releasedAfter.Value)
                {
                    skippedCards++;
                    continue;
                }
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

    private static DateTime GetSetReleaseDate(IScryfallSet set)
    {
        try
        {
            dynamic data = set.Data();
            string releasedAt = data.released_at;
            if (string.IsNullOrEmpty(releasedAt) is false && DateTime.TryParse(releasedAt, out DateTime parsedDate))
            {
                return parsedDate;
            }
        }
        catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException)
        {
            return DateTime.MinValue;
        }

        return DateTime.MinValue;
    }

    public void ProcessCards(IReadOnlyList<IScryfallCard> cards, IReadOnlyDictionary<string, dynamic> setsByCode) => _dashboard.LogProcessingCards(cards.Count);// Card processing logic (if any) goes here

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
            ScryfallCardItemExtEntity cardItem = new()
            {
                Data = card.Data()
            };
            await _cardScribe.UpsertAsync(cardItem).ConfigureAwait(false);

            // Write the card by name (for name-based lookups)
            CardNameGuid nameGuid = _guidGenerator.GenerateGuid((string)card.Data().name);
            ScryfallCardByNameExtEntity cardByNameItem = new()
            {
                NameGuid = nameGuid.AsSystemType().ToString(),
                Data = card.Data()
            };
            await _cardsByNameScribe.UpsertAsync(cardByNameItem).ConfigureAwait(false);

            // Write the set-card relationship
            ScryfallSetCardItemExtEntity setCardItem = _setCardMapper.Map(card.Data());
            await _setCardsScribe.UpsertAsync(setCardItem).ConfigureAwait(false);
        }

        _dashboard.LogCardsWritten(cards.Count);
        _dashboard.UpdateCompletedCount("Cards", cards.Count);
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
        Message = "Writing {Count} cards, cards-by-name, and set-card relationships to Cosmos DB")]
    public static partial void LogWritingCards(this ILogger logger, int count);

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Successfully wrote {Count} cards, cards-by-name, and set-card relationships to Cosmos DB")]
    public static partial void LogCardsWritten(this ILogger logger, int count);

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Skipped {Count} cards due to set filter configuration")]
    public static partial void LogCardsSkipped(this ILogger logger, int count);
}
