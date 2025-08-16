using Example.Core;
using Lib.BlobStorage.Apis;
using Lib.Scryfall.Ingestion.Apis.Collections;
using Lib.Scryfall.Ingestion.Cosmos.Mappers;
using Lib.Scryfall.Ingestion.Cosmos.Operators;
using Lib.Scryfall.Ingestion.Cosmos.Processors;
using Lib.Scryfall.Ingestion.Icons;
using Lib.Scryfall.Ingestion.Icons.Processors;
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
            NonDigitalScryfallSetCollection scryfallSets = new(GetLogger());

            // Create processors with their dependencies
            ISetItemsProcessor setItemsProcessor = new SetItemsProcessor(
                new ScryfallSetItemsScribe(GetLogger()),
                new ScryfallSetToCosmosMapper(),
                GetLogger());

            ISetAssociationsProcessor setAssociationsProcessor = new SetAssociationsProcessor(
                new ScryfallSetAssociationsScribe(GetLogger()),
                new ScryfallSetToAssociationMapper(),
                GetLogger());

            // Create icon processor with blob storage dependencies
            ServiceLocatorAuthBlobConnectionConfig blobConnectionConfig = new();
            ISetIconContainerDefinition iconContainerDefinition = new SetIconContainerDefinition();
            ISetIconContainerAdapter iconContainerAdapter = new SetIconContainerAdapter(
                GetLogger(),
                iconContainerDefinition,
                blobConnectionConfig);

            ISetIconBlobScribe iconScribe = new SetIconBlobScribe(
                GetLogger(),
                iconContainerAdapter);

            ISetIconDownloader iconDownloader = new SetIconDownloader(
                GetLogger());

            ISetIconProcessor setIconProcessor = new SetIconProcessor(
                iconDownloader,
                iconScribe,
                GetLogger());

            IScryfallIngestionService ingestionService = new ScryfallIngestionService(
                GetLogger(),
                scryfallSets,
                setItemsProcessor,
                setAssociationsProcessor,
                setIconProcessor);

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
