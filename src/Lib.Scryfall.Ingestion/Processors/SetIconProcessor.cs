using System;
using System.Net.Http;
using System.Threading.Tasks;
using Azure.Storage.Blobs.Models;
using Lib.BlobStorage.Apis.Operations;
using Lib.BlobStorage.Apis.Operations.Responses;
using Lib.Scryfall.Ingestion.Icons;
using Lib.Scryfall.Shared.Apis.Models;
using Microsoft.Extensions.Logging;

namespace Lib.Scryfall.Ingestion.Processors;

internal sealed class SetIconProcessor : ISetProcessor
{
    private readonly ISetIconDownloader _downloader;
    private readonly IBlobWriteScribe _scribe;
    private readonly ILogger _logger;

    public SetIconProcessor(ILogger logger)
        : this(
            new SetIconDownloader(logger),
            new SetIconBlobScribe(logger),
            logger)
    {
    }

    private SetIconProcessor(
        ISetIconDownloader downloader,
        IBlobWriteScribe scribe,
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
            _logger.LogProcessingIcon(set.Code(), set.Id());

            byte[] iconData = await _downloader.DownloadIconAsync(set.IconSvgPath()).ConfigureAwait(false);

            SetIconBlobEntity blobEntity = new(set, iconData);
            BlobOpResponse<BlobContentInfo> response = await _scribe.WriteAsync(blobEntity).ConfigureAwait(false);

            LogSuccess(set.Code(), response);
            LogFailure(set.Code(), response);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogIconProcessingError(ex, set.Code());
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogIconProcessingError(ex, set.Code());
        }
    }

    private void LogSuccess(string setCode, BlobOpResponse<BlobContentInfo> response)
    {
        if (response.MissingValue()) return;

        _logger.LogIconStored(setCode);
    }

    private void LogFailure(string setCode, BlobOpResponse<BlobContentInfo> response)
    {
        if (response.HasValue()) return;

        _logger.LogIconStoreFailed(setCode);
    }
}

internal static partial class SetIconProcessorLoggerExtensions
{
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
