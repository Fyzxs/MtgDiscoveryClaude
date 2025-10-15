using System;
using System.Threading.Tasks;
using Example.Core;
using Lib.Scryfall.Ingestion.Apis;
using Microsoft.Extensions.Logging;

namespace Example.Scryfall.CosmosIngestion;

public sealed class ScryfallCosmosIngestionApplication : ExampleApplication
{
    protected override async Task Execute()
    {
        ILogger logger = GetLogger();

        logger.LogInformation("Starting Scryfall to Cosmos DB ingestion (excluding digital sets)");

        try
        {
            IScryfallIngestionService ingestionService = new ScryfallIngestionService(logger);

            await ingestionService.IngestAllSetsAsync().ConfigureAwait(false);

            logger.LogInformation("Ingestion example completed");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Fatal error during ingestion");
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

        return loggerFactory.CreateLogger("Example");
    }
}
