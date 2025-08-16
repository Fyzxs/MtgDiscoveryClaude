using Lib.Scryfall.Ingestion.Apis.Configurations;
using Lib.Scryfall.Ingestion.Configurations;
using Lib.Universal.Configurations;

namespace Lib.Scryfall.Ingestion.Apis;

/// <summary>
/// Provides configuration for Scryfall ingestion using the configuration system.
/// This implementation retrieves Scryfall settings from the application configuration.
/// </summary>
public sealed class ConfigScryfallIngestionConfiguration : IScryfallIngestionConfiguration
{
    private readonly IConfig _config;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigScryfallIngestionConfiguration"/> class
    /// using the default mono-state configuration.
    /// </summary>
    public ConfigScryfallIngestionConfiguration() : this(new MonoStateConfig())
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigScryfallIngestionConfiguration"/> class
    /// with the specified configuration provider.
    /// </summary>
    /// <param name="config">The configuration provider to use for retrieving Scryfall settings.</param>
    private ConfigScryfallIngestionConfiguration(IConfig config)
    {
        _config = config;
    }

    /// <summary>
    /// Gets the processing configuration.
    /// </summary>
    /// <returns>The processing configuration.</returns>
    public IScryfallProcessingConfig ProcessingConfig()
    {
        return new ConfigScryfallProcessingConfig($"{IScryfallIngestionConfiguration.ScryfallIngestionConfigurationKey}:{IScryfallIngestionConfiguration.ProcessingKey}", _config);
    }
}
