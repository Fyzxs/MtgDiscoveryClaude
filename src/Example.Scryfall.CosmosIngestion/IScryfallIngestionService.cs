namespace Example.Scryfall.CosmosIngestion;

public interface IScryfallIngestionService
{
    Task IngestAllSetsAsync();
}
