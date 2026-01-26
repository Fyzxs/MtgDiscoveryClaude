namespace Lib.Cosmos.Apis.Configurations;

/// <summary>
/// Defines the configuration for an Azure Cosmos DB database.
/// </summary>
public interface ICosmosDatabaseConfig
{
    /// <summary>
    /// Configuration key for the throughput mode.
    /// </summary>
    const string ThroughputModeKey = "throughput_mode";

    /// <summary>
    /// Configuration key for the database-level autoscale maximum throughput.
    /// </summary>
    const string AutoscaleMaxKey = "autoscale_max";

    /// <summary>
    /// Gets the container configuration for the specified container definition.
    /// </summary>
    /// <param name="cosmosContainerDefinition">The container definition.</param>
    /// <returns>The container configuration.</returns>
    ICosmosContainerConfig ContainerConfig(ICosmosContainerDefinition cosmosContainerDefinition);

    /// <summary>
    /// Gets the throughput mode indicating whether shared database-level or dedicated container-level throughput is used.
    /// </summary>
    /// <returns>The throughput mode.</returns>
    ICosmosThroughputMode ThroughputMode();

    /// <summary>
    /// Gets the database-level autoscale maximum throughput.
    /// Only valid when ThroughputMode returns true.
    /// </summary>
    /// <returns>The database autoscale maximum throughput.</returns>
    CosmosAutoscaleMax AutoscaleMax();
}
