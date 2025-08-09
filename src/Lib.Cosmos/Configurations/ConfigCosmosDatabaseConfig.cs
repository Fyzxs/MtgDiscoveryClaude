using Lib.Cosmos.Apis.Configurations;
using Lib.Universal.Configurations;

namespace Lib.Cosmos.Configurations;

internal sealed class ConfigCosmosDatabaseConfig : ICosmosDatabaseConfig
{
    private readonly string _parentKey;
    private readonly IConfig _config;

    public ConfigCosmosDatabaseConfig(string parentKey, IConfig config)
    {
        _parentKey = parentKey;
        _config = config;
    }

    public ICosmosContainerConfig ContainerConfig(ICosmosContainerDefinition cosmosContainerDefinition)
    {
        string containerKey = $"{_parentKey}:{cosmosContainerDefinition.ContainerName().AsSystemType()}";
        return new ConfigCosmosContainerConfig(containerKey, _config);
    }
}
