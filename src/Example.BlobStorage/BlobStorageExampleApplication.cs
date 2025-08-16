using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Example.Core;
using Lib.BlobStorage.Apis;
using Lib.BlobStorage.Apis.Ids;
using Microsoft.Extensions.Logging;

namespace Example.BlobStorage;

public sealed class BlobStorageExampleApplication : ExampleApplication
{
    protected override async Task Execute()
    {
        ILogger<BlobStorageExampleApplication> logger = GetLogger<BlobStorageExampleApplication>();

        logger.LogInformation("Starting Blob Storage Example");

        try
        {
            ServiceLocatorAuthBlobConnectionConfig connectionConfig = new();
            IDemoContainerDefinition containerDefinition = new DemoContainerDefinition();

            IDemoContainerAdapter containerAdapter = new DemoContainerAdapter(
                GetLogger<DemoContainerAdapter>(),
                containerDefinition,
                connectionConfig);

            IDemoBlobScribe scribe = new DemoBlobScribe(containerAdapter);
            IDemoBlobInquisitor inquisitor = new DemoBlobInquisitor(containerAdapter);
            IDemoBlobListMaker listMaker = new DemoBlobListMaker(containerAdapter);

            await DemoWriteAsync(scribe, logger).ConfigureAwait(false);
            await DemoExistsAsync(inquisitor, logger).ConfigureAwait(false);
            await DemoListAsync(listMaker, logger).ConfigureAwait(false);

            logger.LogInformation("Blob Storage Example completed");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Fatal error during blob storage example");
            throw;
        }
    }

    private async Task DemoWriteAsync(IDemoBlobScribe scribe, ILogger logger)
    {
        logger.LogInformation("Writing test blob...");

        IDemoBlobEntity entity = new DemoBlobEntity("test-file.txt", "Hello from Example.BlobStorage!");
        Lib.BlobStorage.Apis.Operations.Responses.BlobOpResponse<Azure.Storage.Blobs.Models.BlobContentInfo> response =
            await scribe.WriteAsync(entity).ConfigureAwait(false);

        if (response.HasValue())
        {
            logger.LogInformation("Successfully wrote blob: test-file.txt");
        }
        else
        {
            logger.LogError("Failed to write blob");
        }
    }

    private async Task DemoExistsAsync(IDemoBlobInquisitor inquisitor, ILogger logger)
    {
        logger.LogInformation("Checking if blob exists...");

        ProvidedBlobPathEntity path = new("test-file.txt");
        Lib.BlobStorage.Apis.Operations.Responses.BlobOpResponse<bool> response =
            await inquisitor.ExistsAsync(path).ConfigureAwait(false);

        if (response.HasValue())
        {
            logger.LogInformation("Blob exists: {Exists}", response.Value);
        }
        else
        {
            logger.LogError("Failed to check blob existence");
        }
    }

    private async Task DemoListAsync(IDemoBlobListMaker listMaker, ILogger logger)
    {
        logger.LogInformation("Listing blobs in container...");

        IList<Lib.BlobStorage.Operations.IBlobListingItem> blobs = await listMaker.BlobListAsync().ConfigureAwait(false);

        logger.LogInformation("Found {Count} blobs in container", blobs.Count);
        foreach (Lib.BlobStorage.Operations.IBlobListingItem blob in blobs)
        {
            logger.LogInformation("  - {Path}", blob.Path);
        }
    }

    private ILogger<T> GetLogger<T>()
    {
        using ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.SetMinimumLevel(LogLevel.Information);
            builder.AddConsole();
        });

        return loggerFactory.CreateLogger<T>();
    }
}
