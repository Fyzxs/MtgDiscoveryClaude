namespace Lib.Scryfall.Ingestion.Internal.Configurations;
internal interface IScryfallIngestionConfiguration
{
    const string ScryfallIngestionConfigurationKey = "ScryfallIngestionConfiguration";
    const string ProcessingKey = "processing";
    IScryfallProcessingConfig ProcessingConfig();
}
