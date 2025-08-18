using Lib.Cosmos.Configurations;
using Lib.Universal.Configurations;
using Lib.Universal.Extensions;

namespace Lib.Scryfall.Ingestion.Configurations;

internal sealed class ConfigProcessSetsInReverse : ProcessSetsInReverse
{
    private readonly string _sourceKey;
    private readonly IConfig _config;

    public ConfigProcessSetsInReverse(string sourceKey, IConfig config)
    {
        _sourceKey = sourceKey;
        _config = config;
    }

    public override bool AsSystemType()
    {
        string value = _config[_sourceKey];
        if (value.IzNullOrWhiteSpace()) throw new CosmosConfigurationException($"{GetType().Name} requires key [{_sourceKey}]");
        if (bool.TryParse(value, out bool parsed) is false) throw new CosmosConfigurationException($"{GetType().Name} Invalid value [{_sourceKey}={value}]");
        return parsed;
    }
}
