using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Lib.BlobStorage.Adapters;
using Lib.BlobStorage.Apis.Configurations;
using Lib.BlobStorage.Apis.Ids;
using Lib.BlobStorage.Apis.Operations;
using Lib.BlobStorage.Apis.Operations.Responses;
using Lib.BlobStorage.Operations.Responses;
using Microsoft.Extensions.Logging;

namespace Lib.BlobStorage.Operations;

internal sealed class BlobContainerWriteOperator : IBlobContainerWriteOperator
{
    private readonly ILogger _logger;
    private readonly IBlobServiceClientAdapter _clientAdapter;

    public BlobContainerWriteOperator(ILogger logger, IBlobContainerDefinition containerConfig, IBlobConnectionConvenience connectionConfig)
        : this(logger, new MonoStateBlobClientAdapter(containerConfig, connectionConfig))
    { }

    private BlobContainerWriteOperator(ILogger logger, IBlobServiceClientAdapter clientAdapter)
    {
        _logger = logger;
        _clientAdapter = clientAdapter;
    }

    public async Task<BlobOpResponse<BlobContentInfo>> WriteAsync(IBlobBinaryWriteDomain blobWriteEntity)
    {
        BlobContainerClient containerClient = await _clientAdapter.BlobContainerClient().ConfigureAwait(false);
        BlobClient blobClient = containerClient.GetBlobClient(blobWriteEntity.FilePath);

        long swStart = Stopwatch.GetTimestamp();
        try
        {
            BlobUploadOptions uploadOptions = new()
            {
                Metadata = blobWriteEntity.Metadata
            };

            Response<BlobContentInfo> uploadAsync = await blobClient.UploadAsync(blobWriteEntity.Content, uploadOptions).ConfigureAwait(false);

            _logger.WriteInformation("Success", Stopwatch.GetElapsedTime(swStart));
            return new ResponseBlobOpResponse<BlobContentInfo>(uploadAsync);
        }
        catch (RequestFailedException ex)
        {
            _logger.WriteErrorInformation(ex, "Fail", Stopwatch.GetElapsedTime(swStart));
            return new FailureBlobOpResponse<BlobContentInfo>();
        }
    }
}

internal static partial class BlobContainerWriteOperatorLoggerExtensions
{
    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "WriteAsync log:[Status={status}] [ElapsedTime={elapsedTime}]")]
    public static partial void WriteInformation(this ILogger logger, string status, TimeSpan elapsedTime);

    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "WriteAsync log:[Status={status}] [ElapsedTime={elapsedTime}]")]
    public static partial void WriteErrorInformation(this ILogger logger, Exception ex, string status, TimeSpan elapsedTime);
}
