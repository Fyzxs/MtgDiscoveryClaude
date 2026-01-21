using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Lib.Scryfall.Ingestion.Http;

namespace Lib.Scryfall.Ingestion.BulkIngestion;

internal sealed class RulingsBulkDataFetcher
{
    private readonly ILogger _logger;
    private readonly RateLimitedHttpClient _httpClient;
    private const string BulkDataEndpoint = "https://api.scryfall.com/bulk-data";

    public RulingsBulkDataFetcher(ILogger logger)
    {
        _logger = logger;
        _httpClient = new RateLimitedHttpClient();
    }

    public async Task<IAsyncEnumerable<dynamic>> FetchRulingsAsync()
    {
        _logger.LogFetchingBulkDataInfo();

        // Get the bulk data catalog
        dynamic catalog = await _httpClient.ResponseAs<dynamic>(new Uri(BulkDataEndpoint)).ConfigureAwait(false);

        // Find the rulings bulk data entry
        string rulingsUrl = null;
        foreach (dynamic item in catalog.data)
        {
            if (item.type == "rulings")
            {
                rulingsUrl = item.download_uri;
                break;
            }
        }

        if (rulingsUrl is null)
        {
            _logger.LogRulingsBulkDataNotFound();
            return EmptyRulings();
        }

        _logger.LogDownloadingRulingsData(rulingsUrl);

        // Download and parse the rulings file
        return ParseRulingsFile(rulingsUrl);
    }

    private async IAsyncEnumerable<dynamic> ParseRulingsFile(string url)
    {
        Stream stream = await _httpClient.StreamAsync(new Uri(url)).ConfigureAwait(false);
        await using ConfiguredAsyncDisposable _ = stream.ConfigureAwait(false);
        using StreamReader reader = new(stream);
#pragma warning disable CA2007 // It's a constructor
        using JsonTextReader jsonReader = new(reader);
#pragma warning restore CA2007

        JsonSerializer serializer = new();

        // Read the array start
        await jsonReader.ReadAsync().ConfigureAwait(false);
        if (jsonReader.TokenType is not JsonToken.StartArray)
        {
            _logger.LogInvalidRulingsFormat();
            yield break;
        }

        // Read each ruling
        while (await jsonReader.ReadAsync().ConfigureAwait(false))
        {
            if (jsonReader.TokenType == JsonToken.EndArray) { break; }

            dynamic ruling = serializer.Deserialize<dynamic>(jsonReader);
            if (ruling is not null)
            {
                yield return ruling;
            }
        }
    }

    private static async IAsyncEnumerable<dynamic> EmptyRulings()
    {
        await Task.CompletedTask.ConfigureAwait(false);
        yield break;
    }
}

internal static partial class RulingsBulkDataFetcherLoggerExtensions
{
    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Fetching bulk data catalog from Scryfall")]
    public static partial void LogFetchingBulkDataInfo(this ILogger logger);

    [LoggerMessage(
        Level = LogLevel.Warning,
        Message = "Rulings bulk data not found in catalog")]
    public static partial void LogRulingsBulkDataNotFound(this ILogger logger);

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Downloading rulings data from {Url}")]
    public static partial void LogDownloadingRulingsData(this ILogger logger, string url);

    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "Invalid rulings file format - expected JSON array")]
    public static partial void LogInvalidRulingsFormat(this ILogger logger);
}
