using Lib.Cosmos.Apis.Configurations;
using Lib.Universal.Configurations;

namespace Lib.Cosmos.Configurations;

internal sealed class ConfigCosmosSasConnectionConfig : ICosmosSasConnectionConfig
{
    private readonly string _parentKey;
    private readonly IConfig _config;

    public ConfigCosmosSasConnectionConfig(string parentKey, IConfig config)
    {
        _parentKey = parentKey;
        _config = config;
    }

    public CosmosConnectionString ConnectionString()
    {
        string connectionStringKey = $"{_parentKey}:{ICosmosSasConnectionConfig.ConnectionStringKey}";
        return new ConfigCosmosConnectionString(connectionStringKey, _config);
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
