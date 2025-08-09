using Lib.Cosmos.Apis.Ids;
using Lib.Universal.Configurations;
using Lib.Universal.Extensions;

namespace Lib.Cosmos.Configurations;

internal sealed class ConfigCosmosAccountName : CosmosAccountName
{
    private readonly string _sourceKey;
    private readonly IConfig _config;

    public ConfigCosmosAccountName(string sourceKey, IConfig config)
    {
        _sourceKey = sourceKey;
        _config = config;
    }

    public override string AsSystemType()
    {
        string value = _config[_sourceKey];
        if (value.IzNullOrWhiteSpace()) throw new CosmosConfigurationException($"{GetType().Name} requires key [{_sourceKey}]");
        return value;
    }
}
