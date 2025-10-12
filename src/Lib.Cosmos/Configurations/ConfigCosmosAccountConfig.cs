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

    public CosmosClientAuthMode AuthMode() => new ConfigCosmosClientAuthMode($"{_parentKey}:{ICosmosAccountConfig.AuthModeKey}", _config);

    public CosmosAccountName AccountName() => new ConfigCosmosAccountName($"{_parentKey}:{ICosmosAccountConfig.AccountNameKey}", _config);

    public ICosmosDatabaseConfig DatabaseConfig(ICosmosContainerDefinition cosmosContainerDefinition) => new ConfigCosmosDatabaseConfig($"{_parentKey}:{cosmosContainerDefinition.DatabaseName().AsSystemType()}", _config);

    public ICosmosEntraConfig EntraConfig() => new ConfigCosmosEntraConfig($"{_parentKey}:{ICosmosAccountConfig.EntraKey}", _config);

    public ICosmosSasConfig SasConfig() => new ConfigCosmosSasConfig($"{_parentKey}:{ICosmosAccountConfig.SasKey}", _config);
}
