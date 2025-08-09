using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Lib.BlobStorage.Adapters;
using Lib.BlobStorage.Apis.Configurations;
using Lib.BlobStorage.Apis.Operations;
using Lib.Universal.Extensions;
using Microsoft.Extensions.Logging;

namespace Lib.BlobStorage.Operations;

internal sealed class BlobContainerListOperator : IBlobContainerListOperator
{
    private readonly ILogger _logger;
    private readonly IBlobServiceClientAdapter _clientAdapter;

    public BlobContainerListOperator(ILogger logger, IBlobContainerDefinition containerConfig, IBlobConnectionConvenience connectionConfig)
        : this(logger, new MonoStateBlobClientAdapter(containerConfig, connectionConfig))
    { }

    private BlobContainerListOperator(ILogger logger, IBlobServiceClientAdapter clientAdapter)
    {
        _logger = logger;
        _clientAdapter = clientAdapter;
    }

    public async Task<IList<IBlobListingItem>> BlobListAsync()
    {
        BlobContainerClient containerClient = await _clientAdapter.BlobContainerClient().ConfigureAwait(false);

        long swStart = Stopwatch.GetTimestamp();
        List<IBlobListingItem> results = [];

        ConfiguredCancelableAsyncEnumerable<BlobHierarchyItem> items = containerClient.GetBlobsByHierarchyAsync().ConfigureAwait(false);

        try
        {
            await foreach (BlobHierarchyItem item in items)
            {
                if (item.IsBlob is false) continue;
                if (item.Blob.Deleted) continue;

                BlobClient blobClient = containerClient.GetBlobClient(item.Blob.Name);
                BlobProperties properties = await blobClient.GetPropertiesAsync().ConfigureAwait(false);
                BlobListingItem listingItem = new()
                {
                    Metadata = properties.Metadata,
                    Path = item.Blob.Name
                };
                results.Add(listingItem);
            }

            _logger.ListInformation("Success", Stopwatch.GetElapsedTime(swStart), results.Count);
            return results;
        }
        catch (RequestFailedException ex)
        {
            _logger.ListErrorInformation(ex, "Fail", Stopwatch.GetElapsedTime(swStart));
            throw ex.ThrowMe();
        }
    }
}

internal static partial class BlobContainerListOperatorLoggerExtensions
{
    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "BlobListAsync log:[Status={status}] [ElapsedTime={elapsedTime}] [ItemCount={itemCount}]")]
    public static partial void ListInformation(this ILogger logger, string status, TimeSpan elapsedTime, int itemCount);

    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "BlobListAsync log:[Status={status}] [ElapsedTime={elapsedTime}]")]
    public static partial void ListErrorInformation(this ILogger logger, Exception ex, string status, TimeSpan elapsedTime);
}
