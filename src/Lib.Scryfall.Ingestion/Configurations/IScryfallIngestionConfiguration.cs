namespace Lib.Scryfall.Ingestion.Configurations;
internal interface IScryfallIngestionConfiguration
{
    const string ScryfallIngestionConfigurationKey = "ScryfallIngestionConfiguration";
    const string ProcessingKey = "processing";
    IScryfallProcessingConfig ProcessingConfig();
}
