using Lib.Cosmos.Apis.Ids;
using Lib.Universal.Configurations;
using Lib.Universal.Extensions;

namespace Lib.Cosmos.Configurations;

internal sealed class ConfigCosmosPartitionKeyPath : CosmosPartitionKeyPath
{
    private readonly string _sourceKey;
    private readonly IConfig _config;

    public ConfigCosmosPartitionKeyPath(string sourceKey, IConfig config)
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
