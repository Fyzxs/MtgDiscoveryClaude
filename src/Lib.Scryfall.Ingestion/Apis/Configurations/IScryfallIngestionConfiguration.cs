namespace Lib.Scryfall.Ingestion.Apis.Configurations;

/// <summary>
/// Provides configuration for Scryfall ingestion.
/// </summary>
public interface IScryfallIngestionConfiguration
{
    /// <summary>
    /// The key used to locate Scryfall ingestion configuration in the application settings.
    /// </summary>
    const string ScryfallIngestionConfigurationKey = "ScryfallIngestionConfiguration";

    /// <summary>
    /// 
    /// </summary>
    const string ProcessingKey = "processing";

    /// <summary>
    /// Gets the processing configuration.
    /// </summary>
    IScryfallProcessingConfig ProcessingConfig();
}
