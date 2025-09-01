using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Lib.Scryfall.Ingestion.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Lib.Scryfall.Ingestion.BulkIngestion;

internal sealed class CardsBulkDataFetcher
{
    private readonly ILogger _logger;
    private readonly RateLimitedHttpClient _httpClient;
    private const string BulkDataEndpoint = "https://api.scryfall.com/bulk-data";

    public CardsBulkDataFetcher(ILogger logger)
    {
        _logger = logger;
        _httpClient = new RateLimitedHttpClient();
    }

    public async Task<IAsyncEnumerable<dynamic>> FetchCardsAsync()
    {
        _logger.LogFetchingCardsBulkDataInfo();

        // Get the bulk data catalog
        dynamic catalog = await _httpClient.ResponseAs<dynamic>(new Uri(BulkDataEndpoint)).ConfigureAwait(false);

        // Find the default cards bulk data entry
        string cardsUrl = null;
        foreach (dynamic item in catalog.data)
        {
            if (item.type == "default_cards")
            {
                cardsUrl = item.download_uri;
                break;
            }
        }

        if (cardsUrl is null)
        {
            _logger.LogCardsBulkDataNotFound();
            return EmptyCards();
        }

        _logger.LogDownloadingCardsData(cardsUrl);

        // Download and parse the cards file
        return ParseCardsFile(cardsUrl);
    }

    private async IAsyncEnumerable<dynamic> ParseCardsFile(string url)
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
                yield return card;
            }
        }
    }

    private static async IAsyncEnumerable<dynamic> EmptyCards()
    {
        await Task.CompletedTask.ConfigureAwait(false);
        yield break;
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