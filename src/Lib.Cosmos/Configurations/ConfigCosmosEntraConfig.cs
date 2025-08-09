using Lib.Cosmos.Apis.Configurations;
using Lib.Universal.Configurations;

namespace Lib.Cosmos.Configurations;

internal sealed class ConfigCosmosEntraConfig : ICosmosEntraConfig
{
    private readonly string _parentKey;
    private readonly IConfig _config;

    public ConfigCosmosEntraConfig(string parentKey, IConfig config)
    {
        _parentKey = parentKey;
        _config = config;
    }

    public ICosmosEntraConnectionConfig ConnectionConfig()
    {
        string connectionKey = $"{_parentKey}:{ICosmosEntraConfig.ConnectionKey}";
        return new ConfigCosmosEntraConnectionConfig(connectionKey, _config);
    }

    public ICosmosEntraGenesisConfig GenesisConfig()
    {
        string genesisKey = $"{_parentKey}:{ICosmosEntraConfig.GenesisKey}";
        return new ConfigCosmosEntraGenesisConfig(genesisKey, _config);
    }
}
