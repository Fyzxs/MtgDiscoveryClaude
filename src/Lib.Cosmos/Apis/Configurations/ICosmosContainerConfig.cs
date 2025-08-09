namespace Lib.Cosmos.Apis.Configurations;

/// <summary>
/// Defines the optional configuration settings for an Azure Cosmos DB container.
/// </summary>
public interface ICosmosContainerConfig
{
    /// <summary>
    /// Configuration key for the autoscale maximum throughput.
    /// </summary>
    const string AutoscaleMaxKey = "autoscale_max";

    /// <summary>
    /// Configuration key for the time-to-live in seconds.
    /// </summary>
    const string TimeToLiveKey = "time_to_live_seconds";

    /// <summary>
    /// Gets the maximum throughput for autoscale.
    /// </summary>
    /// <returns>The autoscale maximum throughput.</returns>
    CosmosContainerAutoscaleMax AutoscaleMax();

    /// <summary>
    /// Gets the time-to-live (TTL) in seconds for items in the container.
    /// </summary>
    /// <returns>The TTL in seconds.</returns>
    CosmosContainerTimeToLive TimeToLive();
}
