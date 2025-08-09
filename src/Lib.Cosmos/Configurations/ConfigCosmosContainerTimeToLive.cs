using Lib.Cosmos.Apis.Configurations;
using Lib.Universal.Configurations;

namespace Lib.Cosmos.Configurations;

internal sealed class ConfigCosmosContainerTimeToLive : CosmosContainerTimeToLive
{
    private readonly string _sourceKey;
    private readonly IConfig _config;

    public ConfigCosmosContainerTimeToLive(string sourceKey, IConfig config)
    {
        _sourceKey = sourceKey;
        _config = config;
    }

    public override int? AsSystemType()
    {
        string value = _config[_sourceKey];
        if (value is null) return null;
        if (int.TryParse(value, out int parsed) is false) throw new CosmosConfigurationException($"{GetType().Name} Invalid value [{_sourceKey}={value}]");
        return parsed;
    }
}
