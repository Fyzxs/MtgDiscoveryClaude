using Lib.Universal.Configurations;

namespace Lib.Scryfall.Ingestion.Configurations;

internal sealed class ConfigScryfallIngestionConfiguration : IScryfallIngestionConfiguration
{
    private readonly IConfig _config;
    public ConfigScryfallIngestionConfiguration() : this(new MonoStateConfig())
    {
    }
    private ConfigScryfallIngestionConfiguration(IConfig config) => _config = config;
    public IScryfallProcessingConfig ProcessingConfig() => new ConfigScryfallProcessingConfig($"{IScryfallIngestionConfiguration.ScryfallIngestionConfigurationKey}:{IScryfallIngestionConfiguration.ProcessingKey}", _config);
}
