using Lib.Cosmos.Apis.Configurations;
using Lib.Universal.Configurations;
using Lib.Universal.Extensions;

namespace Lib.Cosmos.Configurations;

internal sealed class ConfigCosmosEntraGenesisConfig : ICosmosEntraGenesisConfig
{
    private readonly string _parentKey;
    private readonly IConfig _config;

    public ConfigCosmosEntraGenesisConfig(string parentKey, IConfig config)
    {
        _parentKey = parentKey;
        _config = config;
    }

    public bool Bypass()
    {
        string bypassKey = $"{_parentKey}:{ICosmosGenesisConfig.BypassKey}";
        string value = _config[bypassKey];
        if (value.IzNullOrWhiteSpace()) throw new CosmosConfigurationException($"{GetType().Name} requires key [{bypassKey}]");
        return bool.TryParse(value, out bool result) && result;
    }

    public string SubscriptionId()
    {
        string subscriptionKey = $"{_parentKey}:{ICosmosEntraGenesisConfig.SubscriptionIdKey}";
        string value = _config[subscriptionKey];
        if (value.IzNullOrWhiteSpace()) throw new CosmosConfigurationException($"{GetType().Name} requires key [{subscriptionKey}]");
        return value;
    }

    public string ResourceGroupName()
    {
        string resourceGroupKey = $"{_parentKey}:{ICosmosEntraGenesisConfig.ResourceGroupNameKey}";
        string value = _config[resourceGroupKey];
        if (value.IzNullOrWhiteSpace()) throw new CosmosConfigurationException($"{GetType().Name} requires key [{resourceGroupKey}]");
        return value;
    }
}
