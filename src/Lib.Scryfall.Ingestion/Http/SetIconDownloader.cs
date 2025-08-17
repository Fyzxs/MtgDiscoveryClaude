using System;
using System.IO;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Lib.Universal.Http;
using Lib.Universal.Primitives;
using Microsoft.Extensions.Logging;

namespace Lib.Scryfall.Ingestion.Icons;

internal interface IDownloader
{
    Task<byte[]> DownloadAsync(Url url);
}

internal sealed class HttpDownloader : IDownloader
{
    private readonly IHttpClient _httpClient;
    private readonly ILogger _logger;

    public HttpDownloader(ILogger logger) : this(new MonoStateHttpClient(), logger)
    {
    }

    private HttpDownloader(IHttpClient httpClient, ILogger logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<byte[]> DownloadAsync(Url url)
    {
        try
        {
            _logger.LogDownloading(url.AsSystemType().AbsolutePath);

            Stream stream = await _httpClient.StreamAsync(url).ConfigureAwait(false);
            await using ConfiguredAsyncDisposable _ = stream.ConfigureAwait(false);

            using MemoryStream memoryStream = new();
            await stream.CopyToAsync(memoryStream).ConfigureAwait(false);
            byte[] iconData = memoryStream.ToArray();

            _logger.LogDownloaded(iconData.Length);

            return iconData;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogDownloadError(ex, url.AsSystemType().AbsolutePath);
            throw;
        }
    }
}

internal static partial class SetIconDownloaderLoggerExtensions
{
    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Downloading from {url}")]
    public static partial void LogDownloading(this ILogger logger, string url);

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Successfully downloaded ({size} bytes)")]
    public static partial void LogDownloaded(this ILogger logger, int size);

    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "Failed to download from {url}")]
    public static partial void LogDownloadError(this ILogger logger, Exception ex, string url);
}
