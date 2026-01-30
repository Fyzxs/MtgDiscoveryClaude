using Azure.Core;

namespace Lib.Cosmos.Apis.Configurations;

/// <summary>
/// Provides convenience methods for accessing Cosmos DB configuration and credentials
/// for a given container definition.
/// </summary>
public interface ICosmosConnectionConvenience
{
    /// <summary>
    /// Gets the Cosmos DB account configuration for the specified container definition.
    /// </summary>
    /// <param name="cosmosContainerDefinition">The container definition.</param>
    /// <returns>The account configuration.</returns>
    ICosmosAccountConfig AccountConfig(ICosmosContainerDefinition cosmosContainerDefinition);

    /// <summary>
    /// Gets the Azure Entra (Active Directory) credential for the specified container definition.
    /// </summary>
    /// <param name="cosmosContainerDefinition">The container definition.</param>
    /// <returns>The token credential.</returns>
    TokenCredential AccountEntraCredential(ICosmosContainerDefinition cosmosContainerDefinition);

    /// <summary>
    /// Gets the Entra genesis configuration for the specified container definition.
    /// </summary>
    /// <param name="cosmosContainerDefinition">The container definition.</param>
    /// <returns>The Entra genesis configuration.</returns>
    ICosmosEntraGenesisConfig EntraGenesisConfig(ICosmosContainerDefinition cosmosContainerDefinition);

    /// <summary>
    /// Gets the database configuration for the specified container definition.
    /// </summary>
    /// <param name="cosmosContainerDefinition">The container definition.</param>
    /// <returns>The database configuration.</returns>
    ICosmosDatabaseConfig DatabaseConfig(ICosmosContainerDefinition cosmosContainerDefinition);

    /// <summary>
    /// Gets the container configuration for the specified container definition.
    /// </summary>
    /// <param name="cosmosContainerDefinition">The container definition.</param>
    /// <returns>The container configuration.</returns>
    ICosmosContainerConfig ContainerConfig(ICosmosContainerDefinition cosmosContainerDefinition);
}
