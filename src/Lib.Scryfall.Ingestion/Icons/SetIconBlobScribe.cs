using System.Threading.Tasks;
using Azure.Storage.Blobs.Models;
using Lib.BlobStorage.Apis;
using Lib.BlobStorage.Apis.Operations;
using Lib.BlobStorage.Apis.Operations.Responses;
using Microsoft.Extensions.Logging;

namespace Lib.Scryfall.Ingestion.Icons;

public interface ISetIconBlobScribe : IBlobWriteScribe
{
    Task<BlobOpResponse<BlobContentInfo>> WriteSetIconAsync(string setId, string setCode, byte[] iconData);
}

internal sealed class SetIconBlobScribe : BlobWriteScribe, ISetIconBlobScribe
{
    private readonly ILogger _logger;

    public SetIconBlobScribe(ILogger<SetIconBlobScribe> logger, ISetIconContainerAdapter containerAdapter)
        : base(containerAdapter)
    {
        _logger = logger;
    }

    public async Task<BlobOpResponse<BlobContentInfo>> WriteSetIconAsync(string setId, string setCode, byte[] iconData)
    {
        SetIconBlobEntity entity = new(setId, setCode, iconData);
        BlobOpResponse<BlobContentInfo> response = await WriteAsync(entity).ConfigureAwait(false);

        if (response.HasValue())
        {
            _logger.LogIconWriteSuccess(setId);
        }
        else
        {
            _logger.LogIconWriteError(setId);
        }

        return response;
    }
}

internal static partial class SetIconBlobScribeLoggerExtensions
{
    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Successfully wrote set icon to id/{SetId}.svg")]
    public static partial void LogIconWriteSuccess(this ILogger logger, string setId);

    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "Failed to write set icon to id/{SetId}.svg")]
    public static partial void LogIconWriteError(this ILogger logger, string setId);
}