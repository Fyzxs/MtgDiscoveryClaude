using Lib.Cosmos.Apis.Configurations;
using Lib.Cosmos.Apis.Ids;
using Lib.Universal.Configurations;

namespace Lib.Cosmos.Configurations;

internal sealed class ConfigCosmosAccountConfig : ICosmosAccountConfig
{
    private readonly string _parentKey;
    private readonly IConfig _config;

    public ConfigCosmosAccountConfig(string parentKey, IConfig config)
    {
        _parentKey = parentKey;
        _config = config;
    }

    public CosmosClientAuthMode AuthMode()
    {
        return new ConfigCosmosClientAuthMode($"{_parentKey}:{ICosmosAccountConfig.AuthModeKey}", _config);
    }

    public CosmosAccountName AccountName()
    {
        return new ConfigCosmosAccountName($"{_parentKey}:{ICosmosAccountConfig.AccountNameKey}", _config);
    }

    public ICosmosDatabaseConfig DatabaseConfig(ICosmosContainerDefinition cosmosContainerDefinition)
    {
        return new ConfigCosmosDatabaseConfig($"{_parentKey}:{cosmosContainerDefinition.DatabaseName().AsSystemType()}", _config);
    }

    public ICosmosEntraConfig EntraConfig()
    {
        return new ConfigCosmosEntraConfig($"{_parentKey}:{ICosmosAccountConfig.EntraKey}", _config);
    }

    public ICosmosSasConfig SasConfig()
    {
        return new ConfigCosmosSasConfig($"{_parentKey}:{ICosmosAccountConfig.SasKey}", _config);
    }
}
