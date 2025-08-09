using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Azure;
using Azure.Storage.Blobs;
using Lib.BlobStorage.Adapters;
using Lib.BlobStorage.Apis.Configurations;
using Lib.BlobStorage.Apis.Ids;
using Lib.BlobStorage.Apis.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.BlobStorage.Operations;

internal sealed class BlobContainerDeleteOperator : IBlobContainerDeleteOperator
{
    private readonly ILogger _logger;
    private readonly IBlobServiceClientAdapter _clientAdapter;

    public BlobContainerDeleteOperator(ILogger logger, IBlobContainerDefinition containerConfig, IBlobConnectionConvenience connectionConfig)
        : this(logger, new MonoStateBlobClientAdapter(containerConfig, connectionConfig))
    { }

    private BlobContainerDeleteOperator(ILogger logger, IBlobServiceClientAdapter clientAdapter)
    {
        _logger = logger;
        _clientAdapter = clientAdapter;
    }

    public async Task DeleteAsync(BlobPathEntity blobItemPath)
    {
        BlobContainerClient containerClient = await _clientAdapter.BlobContainerClient().ConfigureAwait(false);
        BlobClient blobClient = containerClient.GetBlobClient(blobItemPath);
        long swStart = Stopwatch.GetTimestamp();
        try
        {
            _ = await blobClient.DeleteAsync().ConfigureAwait(false);
        }
        catch (RequestFailedException ex) when (ex.Status == 404)
        {
            Console.WriteLine(ex.Message);
            _logger.DeleteInformation(Stopwatch.GetElapsedTime(swStart));
        }
    }
}

internal static partial class BlobContainerDeleteOperatorLoggerExtensions
{
    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "DeleteAsync log: [ElapsedTime={elapsedTime}]")]
    public static partial void DeleteInformation(this ILogger logger, TimeSpan elapsedTime);
}
