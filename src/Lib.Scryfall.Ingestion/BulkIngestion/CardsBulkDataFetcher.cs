using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Lib.Scryfall.Ingestion.Collections;
using Lib.Scryfall.Ingestion.Dtos;
using Lib.Scryfall.Ingestion.Http;
using Lib.Scryfall.Ingestion.Models;
using Lib.Scryfall.Shared.Apis.Models;

namespace Lib.Scryfall.Ingestion.BulkIngestion;

internal sealed class CardsBulkDataFetcher
{
    private readonly ILogger _logger;
    private readonly RateLimitedHttpClient _httpClient;
    private readonly IMonoStateSetsCollection _setsCollection;
    private const string BulkDataEndpoint = "https://api.scryfall.com/bulk-data";

    public CardsBulkDataFetcher(ILogger logger)
    {
        _logger = logger;
        _httpClient = new RateLimitedHttpClient();
        _setsCollection = new MonoStateSetsCollection(logger);
    }

    public async Task<IAsyncEnumerable<IScryfallCard>> FetchCardsAsync()
    {
        _logger.LogFetchingCardsBulkDataInfo();

        // Get the bulk data catalog
        dynamic catalog = await _httpClient.ResponseAs<dynamic>(new Uri(BulkDataEndpoint)).ConfigureAwait(false);

        // Find the default cards bulk data entry
        string cardsUrl = null;
        foreach (dynamic item in catalog.data)
        {
            string type = (string)item.type;
            if ("default_cards".Equals(type) is false) continue;

            cardsUrl = item.download_uri;
            break;
        }

        _logger.LogDownloadingCardsData(cardsUrl!);

        // Download and parse the cards file
        return ParseCardsFile(cardsUrl);
    }

    private async IAsyncEnumerable<IScryfallCard> ParseCardsFile(string url)
    {
        using Stream stream = await _httpClient.StreamAsync(new Uri(url)).ConfigureAwait(false);
        using StreamReader reader = new(stream);
        using JsonTextReader jsonReader = new(reader);

        JsonSerializer serializer = new();

        // Read the array start
        await jsonReader.ReadAsync().ConfigureAwait(false);
        if (jsonReader.TokenType is not JsonToken.StartArray)
        {
            _logger.LogInvalidCardsFormat();
            yield break;
        }

        // Read each card
        while (await jsonReader.ReadAsync().ConfigureAwait(false))
        {
            if (jsonReader.TokenType == JsonToken.EndArray) { break; }

            dynamic card = serializer.Deserialize<dynamic>(jsonReader);
            if (card is not null)
            {
                // Filter out unreleased cards
                if (IsCardReleased(card) is false)
                {
                    continue;
                }

                // Get the actual set from the collection
                string setCode = card.set;
                IScryfallSet set = await _setsCollection.GetSetAsync(setCode).ConfigureAwait(false);
                ExtScryfallCardDto dto = new(card);
                yield return new ScryfallCard(dto, set);
            }
        }
    }

    private static bool IsCardReleased(dynamic card)
    {
        try
        {
            string releasedAt = card.released_at;
            if (string.IsNullOrEmpty(releasedAt))
            {
                // If no release date, assume it's released
                return true;
            }

            if (DateTime.TryParse(releasedAt, out DateTime releaseDate))
            {
                // Card is released if the release date is today or in the past
                return releaseDate.Date <= DateTime.Today;
            }

            // If we can't parse the date, assume it's released to be safe
            return true;
        }
#pragma warning disable CA1031
        catch
#pragma warning restore CA1031
        {
            // If any error occurs, assume it's released to be safe
            return true;
        }
    }
}

internal static partial class CardsBulkDataFetcherLoggerExtensions
{
    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Fetching cards bulk data catalog from Scryfall")]
    public static partial void LogFetchingCardsBulkDataInfo(this ILogger logger);

    [LoggerMessage(
        Level = LogLevel.Warning,
        Message = "Cards bulk data not found in catalog")]
    public static partial void LogCardsBulkDataNotFound(this ILogger logger);

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Downloading cards data from {Url}")]
    public static partial void LogDownloadingCardsData(this ILogger logger, string url);

    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "Invalid cards file format - expected JSON array")]
    public static partial void LogInvalidCardsFormat(this ILogger logger);
}

