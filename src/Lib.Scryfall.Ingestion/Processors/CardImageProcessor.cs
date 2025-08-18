using System;
using System.Net.Http;
using System.Threading.Tasks;
using Azure.Storage.Blobs.Models;
using Lib.Adapter.Scryfall.BlobStorage.Apis.Entities;
using Lib.Adapter.Scryfall.BlobStorage.Apis.Operators;
using Lib.BlobStorage.Apis.Operations;
using Lib.BlobStorage.Apis.Operations.Responses;
using Lib.Scryfall.Ingestion.Configurations;
using Lib.Scryfall.Ingestion.Http;
using Lib.Scryfall.Shared.Apis.Models;
using Microsoft.Extensions.Logging;

namespace Lib.Scryfall.Ingestion.Processors;

internal sealed class CardImageProcessor : ICardProcessor
{
    private readonly IDownloader _downloader;
    private readonly IBlobWriteScribe _scribe;
    private readonly IBlobInquisitor _inquisitor;
    private readonly IScryfallIngestionConfiguration _config;
    private readonly ILogger _logger;

    public CardImageProcessor(ILogger logger)
        : this(
            new HttpDownloader(logger),
            new CardImageBlobScribe(logger),
            new CardImageBlobInquisitor(logger),
            new ConfigScryfallIngestionConfiguration(),
            logger)
    {
    }

    private CardImageProcessor(
        IDownloader downloader,
        IBlobWriteScribe scribe,
        IBlobInquisitor inquisitor,
        ConfigScryfallIngestionConfiguration config,
        ILogger logger)
    {
        _downloader = downloader;
        _scribe = scribe;
        _inquisitor = inquisitor;
        _config = config;
        _logger = logger;
    }

    public async Task ProcessAsync(IScryfallCard card)
    {
        ICardImageInfoCollection imageInfos = card.ImageUris();

        foreach (ICardImageInfo imageInfo in imageInfos)
        {
            await ProcessImageAsync(imageInfo).ConfigureAwait(false);
        }
    }

    private async Task ProcessImageAsync(ICardImageInfo imageInfo)
    {
        try
        {
            _logger.LogProcessingCardImage(imageInfo.LogValue());

            if (await ShouldSkipDownload(imageInfo).ConfigureAwait(false)) return;

            byte[] imageBytes = await _downloader.DownloadAsync(imageInfo.ImageUrl()).ConfigureAwait(false);

            CardImageBlobEntity blobEntity = new(imageInfo, imageBytes);
            BlobOpResponse<BlobContentInfo> response = await _scribe.WriteAsync(blobEntity).ConfigureAwait(false);

            LogSuccess(imageInfo, response);
            LogFailure(imageInfo, response);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogCardImageProcessingError(ex, imageInfo.LogValue());
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogCardImageProcessingError(ex, imageInfo.LogValue());
        }
    }

    private async Task<bool> ShouldSkipDownload(ICardImageInfo imageInfo)
    {
        if (_config.ProcessingConfig().AlwaysDownloadImages()) return false;

        CardImageBlobEntity checkEntity = new(imageInfo, []);
        BlobOpResponse<bool> existsResponse = await _inquisitor.ExistsAsync(checkEntity.FilePath).ConfigureAwait(false);

        if (existsResponse.MissingValue()) return false;

        _logger.LogCardImageAlreadyExists(imageInfo.LogValue());
        return true;
    }

    private void LogSuccess(ICardImageInfo imageInfo, BlobOpResponse<BlobContentInfo> response)
    {
        if (response.MissingValue()) return;

        _logger.LogCardImageStored(imageInfo.LogValue());
    }

    private void LogFailure(ICardImageInfo imageInfo, BlobOpResponse<BlobContentInfo> response)
    {
        if (response.HasValue()) return;

        _logger.LogCardImageStoreFailed(imageInfo.LogValue());
    }
}

internal static partial class CardImageProcessorLoggerExtensions
{
    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Processing image for card {logInfo}")]
    public static partial void LogProcessingCardImage(this ILogger logger, string logInfo);

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Successfully stored image for card {logInfo}")]
    public static partial void LogCardImageStored(this ILogger logger, string logInfo);

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Image already exists for card {logInfo}, skipping download")]
    public static partial void LogCardImageAlreadyExists(this ILogger logger, string logInfo);

    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "Failed to store image for card {logInfo}")]
    public static partial void LogCardImageStoreFailed(this ILogger logger, string logInfo);

    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "Error processing image for card {logInfo}")]
    public static partial void LogCardImageProcessingError(this ILogger logger, Exception ex, string logInfo);
}
