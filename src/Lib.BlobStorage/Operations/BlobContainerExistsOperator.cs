using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Azure;
using Azure.Storage.Blobs;
using Lib.BlobStorage.Adapters;
using Lib.BlobStorage.Apis.Configurations;
using Lib.BlobStorage.Apis.Ids;
using Lib.BlobStorage.Apis.Operations;
using Lib.BlobStorage.Apis.Operations.Responses;
using Lib.BlobStorage.Operations.Responses;
using Microsoft.Extensions.Logging;

namespace Lib.BlobStorage.Operations;

internal sealed class BlobContainerExistsOperator : IBlobContainerExistsOperator
{
    private readonly ILogger _logger;
    private readonly IBlobServiceClientAdapter _clientAdapter;

    public BlobContainerExistsOperator(ILogger logger, IBlobContainerDefinition containerConfig, IBlobConnectionConvenience connectionConfig)
        : this(logger, new MonoStateBlobClientAdapter(containerConfig, connectionConfig))
    { }

    private BlobContainerExistsOperator(ILogger logger, IBlobServiceClientAdapter clientAdapter)
    {
        _logger = logger;
        _clientAdapter = clientAdapter;
    }

    public async Task<BlobOpResponse<bool>> ExistsAsync(BlobPathEntity blobItemPath)
    {
        BlobContainerClient containerClient = await _clientAdapter.BlobContainerClient().ConfigureAwait(false);
        BlobClient blobClient = containerClient.GetBlobClient(blobItemPath);
        long swStart = Stopwatch.GetTimestamp();
        Response<bool> existsResponse = await blobClient.ExistsAsync().ConfigureAwait(false);
        _logger.ExistsInformation(Stopwatch.GetElapsedTime(swStart));
        return new ResponseBlobOpResponse<bool>(existsResponse);
    }
}

internal static partial class BlobContainerExistsOperatorLoggerExtensions
{
    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "ExistsAsync log: [ElapsedTime={elapsedTime}]")]
    public static partial void ExistsInformation(this ILogger logger, TimeSpan elapsedTime);
}
