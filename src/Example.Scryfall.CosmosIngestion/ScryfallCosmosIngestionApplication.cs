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
        ILogger<ScryfallCosmosIngestionApplication> logger = GetLogger<ScryfallCosmosIngestionApplication>();

        logger.LogInformation("Starting Scryfall to Cosmos DB ingestion (excluding digital sets)");

        try
        {
            NonDigitalScryfallSetCollection scryfallSets = new();

            // Create processors with their dependencies
            ISetItemsProcessor setItemsProcessor = new SetItemsProcessor(
                new ScryfallSetItemsScribe(GetLogger<ScryfallSetItemsScribe>()),
                new ScryfallSetToCosmosMapper(),
                GetLogger<SetItemsProcessor>());

            ISetAssociationsProcessor setAssociationsProcessor = new SetAssociationsProcessor(
                new ScryfallSetAssociationsScribe(GetLogger<ScryfallSetAssociationsScribe>()),
                new ScryfallSetToAssociationMapper(),
                GetLogger<SetAssociationsProcessor>());

            // Create icon processor with blob storage dependencies
            ServiceLocatorAuthBlobConnectionConfig blobConnectionConfig = new();
            ISetIconContainerDefinition iconContainerDefinition = new SetIconContainerDefinition();
            ISetIconContainerAdapter iconContainerAdapter = new SetIconContainerAdapter(
                GetLogger<SetIconContainerAdapter>(),
                iconContainerDefinition,
                blobConnectionConfig);

            ISetIconBlobScribe iconScribe = new SetIconBlobScribe(
                GetLogger<SetIconBlobScribe>(),
                iconContainerAdapter);

            ISetIconDownloader iconDownloader = new SetIconDownloader(
                GetLogger<SetIconDownloader>());

            ISetIconProcessor setIconProcessor = new SetIconProcessor(
                iconDownloader,
                iconScribe,
                GetLogger<SetIconProcessor>());

            IScryfallIngestionService ingestionService = new ScryfallIngestionService(
                GetLogger<ScryfallIngestionService>(),
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
