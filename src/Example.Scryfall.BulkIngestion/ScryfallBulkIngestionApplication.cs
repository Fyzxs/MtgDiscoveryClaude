using System;
using System.Threading.Tasks;
using Example.Core;
using Lib.Scryfall.Ingestion.Apis;
using Microsoft.Extensions.Logging;

namespace Example.Scryfall.BulkIngestion;

public sealed class ScryfallBulkIngestionApplication : ExampleApplication
{
    protected override async Task Execute()
    {
        ILogger logger = GetLogger();

        logger.LogInformation("Starting Scryfall bulk data ingestion");

        try
        {
            IBulkIngestionService bulkIngestionService = new BulkIngestionService(logger);

            logger.LogInformation("This will:");
            logger.LogInformation("1. Load filtered sets");
            logger.LogInformation("2. Download and process rulings");
            logger.LogInformation("3. Download and process cards");
            logger.LogInformation("4. Extract artists from cards");
            logger.LogInformation("5. Write data to Cosmos DB");

            await bulkIngestionService.IngestBulkDataAsync().ConfigureAwait(false);

            logger.LogInformation("Bulk ingestion completed successfully");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Fatal error during bulk ingestion");
            throw;
        }
    }

    private ILogger GetLogger()
    {
        using ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.SetMinimumLevel(LogLevel.Information);
            builder.AddConsole();
        });

        return loggerFactory.CreateLogger("BulkIngestion");
    }
}