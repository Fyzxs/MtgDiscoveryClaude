using Lib.Cosmos.Apis.Ids;

namespace Lib.Cosmos.Apis.Configurations;

/// <summary>
/// Defines the configuration needed to create and manage an Azure Cosmos DB container.
/// </summary>
public interface ICosmosContainerDefinition
{
    /// <summary>
    /// Gets the friendly name of the Cosmos DB account.
    /// </summary>
    /// <returns>The account name.</returns>
    CosmosFriendlyAccountName FriendlyAccountName();

    /// <summary>
    /// Gets the name of the database.
    /// </summary>
    /// <returns>The database name.</returns>
    CosmosDatabaseName DatabaseName();

    /// <summary>
    /// Gets the name of the container.
    /// </summary>
    /// <returns>The container name.</returns>
    CosmosContainerName ContainerName();

    /// <summary>
    /// Gets the partition key path for the container.
    /// </summary>
    /// <returns>The partition key path.</returns>
    CosmosPartitionKeyPath PartitionKeyPath();
}
