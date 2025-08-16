using System;
using System.IO;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Lib.Universal.Http;
using Microsoft.Extensions.Logging;

namespace Lib.Scryfall.Ingestion.Icons;

internal interface ISetIconDownloader
{
    Task<byte[]> DownloadIconAsync(string iconUrl);
}

internal sealed class SetIconDownloader : ISetIconDownloader
{
    private readonly IHttpClient _httpClient;
    private readonly ILogger _logger;

    public SetIconDownloader(ILogger logger) : this(new MonoStateHttpClient(), logger)
    {
    }

    private SetIconDownloader(IHttpClient httpClient, ILogger logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<byte[]> DownloadIconAsync(string iconUrl)
    {
        try
        {
            _logger.LogDownloadingIcon(iconUrl);

            Uri uri = new(iconUrl);
            Stream stream = await _httpClient.StreamAsync(uri).ConfigureAwait(false);
            await using ConfiguredAsyncDisposable _ = stream.ConfigureAwait(false);

            using MemoryStream memoryStream = new();
            await stream.CopyToAsync(memoryStream).ConfigureAwait(false);
            byte[] iconData = memoryStream.ToArray();

            _logger.LogIconDownloaded(iconData.Length);

            return iconData;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogIconDownloadError(ex, iconUrl);
            throw;
        }
    }
}

internal static partial class SetIconDownloaderLoggerExtensions
{
    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Downloading icon from {Url}")]
    public static partial void LogDownloadingIcon(this ILogger logger, string url);

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Successfully downloaded icon ({Size} bytes)")]
    public static partial void LogIconDownloaded(this ILogger logger, int size);

    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "Failed to download icon from {Url}")]
    public static partial void LogIconDownloadError(this ILogger logger, Exception ex, string url);
}
