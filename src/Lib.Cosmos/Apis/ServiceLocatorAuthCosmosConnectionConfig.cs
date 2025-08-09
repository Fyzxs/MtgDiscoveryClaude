using Azure.Core;
using Lib.Cosmos.Apis.Configurations;
using Lib.Universal.Inversion;

namespace Lib.Cosmos.Apis;

/// <inheritdoc />
public sealed class ServiceLocatorAuthCosmosConnectionConfig : ICosmosConnectionConvenience
{
    private readonly ICosmosConfiguration _config;
    private readonly IServiceLocator _serviceLocator;

    /// <summary>
    /// Simple constructor
    /// </summary>
    public ServiceLocatorAuthCosmosConnectionConfig() : this(new ConfigCosmosConfiguration(), ServiceLocator.Instance)
    { }

    private ServiceLocatorAuthCosmosConnectionConfig(ICosmosConfiguration config, IServiceLocator serviceLocator)
    {
        _config = config;
        _serviceLocator = serviceLocator;
    }

    /// <inheritdoc />
    public ICosmosAccountConfig AccountConfig(ICosmosContainerDefinition cosmosContainerDefinition) => _config.AccountConfig(cosmosContainerDefinition);

    /// <inheritdoc />
    public TokenCredential AccountEntraCredential(ICosmosContainerDefinition cosmosContainerDefinition) => _serviceLocator.GetService<TokenCredential, TokenCredential>();

    /// <inheritdoc />
    public ICosmosEntraGenesisConfig EntraGenesisConfig(ICosmosContainerDefinition cosmosContainerDefinition) => _config.AccountConfig(cosmosContainerDefinition).EntraConfig().GenesisConfig();

    /// <inheritdoc />
    public ICosmosDatabaseConfig DatabaseConfig(ICosmosContainerDefinition cosmosContainerDefinition) => _config.AccountConfig(cosmosContainerDefinition).DatabaseConfig(cosmosContainerDefinition);

    /// <inheritdoc />
    public ICosmosContainerConfig ContainerConfig(ICosmosContainerDefinition cosmosContainerDefinition) => _config.AccountConfig(cosmosContainerDefinition).DatabaseConfig(cosmosContainerDefinition).ContainerConfig(cosmosContainerDefinition);
}
