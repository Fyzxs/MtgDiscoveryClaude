namespace Lib.Cosmos.Apis.Configurations;

/// <summary>
/// Defines the configuration for an Azure Cosmos DB database.
/// </summary>
public interface ICosmosDatabaseConfig
{
    /// <summary>
    /// Gets the container configuration for the specified container configuration.
    /// </summary>
    /// <param name="cosmosContainerDefinition">The container configuration.</param>
    /// <returns>The container options.</returns>
    ICosmosContainerConfig ContainerConfig(ICosmosContainerDefinition cosmosContainerDefinition);
}
