using Lib.Scryfall.Ingestion.Configurations;
using Lib.Scryfall.Ingestion.Internal.Configurations;
using Lib.Universal.Configurations;

namespace Lib.Scryfall.Ingestion.Apis;
internal sealed class ConfigScryfallIngestionConfiguration : IScryfallIngestionConfiguration
{
    private readonly IConfig _config;
    public ConfigScryfallIngestionConfiguration() : this(new MonoStateConfig())
    {
    }
    private ConfigScryfallIngestionConfiguration(IConfig config)
    {
        _config = config;
    }
    public IScryfallProcessingConfig ProcessingConfig()
    {
        return new ConfigScryfallProcessingConfig($"{IScryfallIngestionConfiguration.ScryfallIngestionConfigurationKey}:{IScryfallIngestionConfiguration.ProcessingKey}", _config);
    }
}
