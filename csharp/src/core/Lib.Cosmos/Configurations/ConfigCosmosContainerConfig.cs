using Lib.Cosmos.Apis.Configurations;
using Lib.Universal.Configurations;

namespace Lib.Cosmos.Configurations;

internal sealed class ConfigCosmosContainerConfig : ICosmosContainerConfig
{
    private readonly string _parentKey;
    private readonly IConfig _config;

    public ConfigCosmosContainerConfig(string parentKey, IConfig config)
    {
        _parentKey = parentKey;
        _config = config;
    }

    public CosmosAutoscaleMax AutoscaleMax() => new ConfigCosmosContainerAutoscaleMax($"{_parentKey}:{ICosmosContainerConfig.AutoscaleMaxKey}", _config);

    public CosmosContainerTimeToLive TimeToLive() => new ConfigCosmosContainerTimeToLive($"{_parentKey}:{ICosmosContainerConfig.TimeToLiveKey}", _config);
}
