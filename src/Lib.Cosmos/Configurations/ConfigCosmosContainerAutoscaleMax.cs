using Lib.Cosmos.Apis.Configurations;
using Lib.Universal.Configurations;
using Lib.Universal.Extensions;

namespace Lib.Cosmos.Configurations;

internal sealed class ConfigCosmosContainerAutoscaleMax : CosmosContainerAutoscaleMax
{
    private readonly string _sourceKey;
    private readonly IConfig _config;

    public ConfigCosmosContainerAutoscaleMax(string sourceKey, IConfig config)
    {
        _sourceKey = sourceKey;
        _config = config;
    }

    public override int AsSystemType()
    {
        string value = _config[_sourceKey];
        if (value.IzNullOrWhiteSpace()) throw new CosmosConfigurationException($"{GetType().Name} requires key [{_sourceKey}]");
        if (int.TryParse(value, out int parsed) is false) throw new CosmosConfigurationException($"{GetType().Name} Invalid value [{_sourceKey}={value}]");
        return parsed;
    }
}
