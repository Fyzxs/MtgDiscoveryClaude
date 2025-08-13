using Example.Core;
using Lib.Scryfall.Ingestion.Apis.Collections;
using Lib.Scryfall.Ingestion.Cosmos.Mappers;
using Lib.Scryfall.Ingestion.Cosmos.Operators;
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
            IScryfallSetItemsScribe scribe = new ScryfallSetItemsScribe(GetLogger<ScryfallSetItemsScribe>());
            IScryfallSetToCosmosMapper mapper = new ScryfallSetToCosmosMapper();

            IScryfallIngestionService ingestionService = new ScryfallIngestionService(
                GetLogger<ScryfallIngestionService>(),
                scryfallSets,
                scribe,
                mapper);

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
