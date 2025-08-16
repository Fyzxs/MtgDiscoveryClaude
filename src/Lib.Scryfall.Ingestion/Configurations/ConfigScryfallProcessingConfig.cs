using Lib.Scryfall.Ingestion.Apis.Configurations;
using Lib.Universal.Configurations;

namespace Lib.Scryfall.Ingestion.Configurations;

internal sealed class ConfigScryfallProcessingConfig : IScryfallProcessingConfig
{
    private readonly string _parentKey;
    private readonly IConfig _config;

    public ConfigScryfallProcessingConfig(string parentKey, IConfig config)
    {
        _parentKey = parentKey;
        _config = config;
    }

    public MaxSetsToProcess MaxSets()
    {
        return new ConfigMaxSetsToProcess($"{_parentKey}:{IScryfallProcessingConfig.MaxSetsKey}", _config);
    }

    public SpecificSetCodes SpecificSets()
    {
        return new ConfigSpecificSetCodes($"{_parentKey}:{IScryfallProcessingConfig.SpecificSetsKey}", _config);
    }
}
