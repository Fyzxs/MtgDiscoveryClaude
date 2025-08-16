using System;
using System.Net.Http;
using System.Threading.Tasks;
using Azure.Storage.Blobs.Models;
using Lib.BlobStorage.Apis.Operations.Responses;
using Lib.Scryfall.Ingestion.Apis.Models;
using Microsoft.Extensions.Logging;

namespace Lib.Scryfall.Ingestion.Icons.Processors;

internal sealed class SetIconProcessor : ISetIconProcessor
{
    private readonly ISetIconDownloader _downloader;
    private readonly ISetIconBlobScribe _scribe;
    private readonly ILogger _logger;

    public SetIconProcessor(
        ISetIconDownloader downloader,
        ISetIconBlobScribe scribe,
        ILogger logger)
    {
        _downloader = downloader;
        _scribe = scribe;
        _logger = logger;
    }

    public async Task ProcessAsync(IScryfallSet set)
    {
        try
        {
            dynamic data = set.Data();

            // Check if icon_svg_uri exists
            if (data.icon_svg_uri == null)
            {
                _logger.LogSetHasNoIcon(set.Code());
                return;
            }

            string iconUrl = data.icon_svg_uri;
            string setId = data.id;
            string setCode = set.Code();

            _logger.LogProcessingIcon(setCode, setId);

            // Download the icon
            byte[] iconData = await _downloader.DownloadIconAsync(iconUrl).ConfigureAwait(false);

            // Store the icon in blob storage
            BlobOpResponse<BlobContentInfo> response =
                await _scribe.WriteSetIconAsync(setId, setCode, iconData).ConfigureAwait(false);

            if (response.HasValue())
            {
                _logger.LogIconStored(setCode);
            }
            else
            {
                _logger.LogIconStoreFailed(setCode);
            }
        }
        catch (HttpRequestException ex)
        {
#pragma warning disable CA1031 // Do not catch general exception types
            _logger.LogIconProcessingError(ex, set.Code());
#pragma warning restore CA1031
            // Don't throw - we don't want icon processing to fail the entire ingestion
        }
        catch (InvalidOperationException ex)
        {
#pragma warning disable CA1031 // Do not catch general exception types
            _logger.LogIconProcessingError(ex, set.Code());
#pragma warning restore CA1031
            // Don't throw - we don't want icon processing to fail the entire ingestion
        }
    }
}

internal static partial class SetIconProcessorLoggerExtensions
{
    [LoggerMessage(
        Level = LogLevel.Warning,
        Message = "Set {Code} has no icon_svg_uri")]
    public static partial void LogSetHasNoIcon(this ILogger logger, string code);

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Processing icon for set {Code} ({Id})")]
    public static partial void LogProcessingIcon(this ILogger logger, string code, string id);

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Successfully stored icon for set {Code}")]
    public static partial void LogIconStored(this ILogger logger, string code);

    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "Failed to store icon for set {Code}")]
    public static partial void LogIconStoreFailed(this ILogger logger, string code);

    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "Error processing icon for set {Code}")]
    public static partial void LogIconProcessingError(this ILogger logger, Exception ex, string code);
}