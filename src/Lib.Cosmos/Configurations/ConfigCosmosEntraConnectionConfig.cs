using Lib.Cosmos.Apis.Configurations;
using Lib.Universal.Configurations;

namespace Lib.Cosmos.Configurations;

internal sealed class ConfigCosmosEntraConnectionConfig : ICosmosEntraConnectionConfig
{
    private readonly string _parentKey;
    private readonly IConfig _config;

    public ConfigCosmosEntraConnectionConfig(string parentKey, IConfig config)
    {
        _parentKey = parentKey;
        _config = config;
    }

    public CosmosAccountEndpoint AccountEndpoint()
    {
        string endpointKey = $"{_parentKey}:{ICosmosEntraConnectionConfig.AccountEndpointKey}";
        return new ConfigCosmosAccountEndpoint(endpointKey, _config);
    }

    public CosmosPreferredRegions PreferredRegions()
    {
        string regionsKey = $"{_parentKey}:{ICosmosConnectionOptionsConfig.PreferredRegionsKey}";
        return new ConfigCosmosPreferredRegions(regionsKey, _config);
    }

    public CosmosConnectionMode ConnectionMode()
    {
        string modeKey = $"{_parentKey}:{ICosmosConnectionOptionsConfig.ConnectionModeKey}";
        return new ConfigCosmosConnectionMode(modeKey, _config);
    }
}
