namespace Lib.Cosmos.Apis.Configurations;

/// <summary>
/// Represents the throughput mode for Azure Cosmos DB.
/// </summary>
public interface ICosmosThroughputMode
{
    /// <summary>
    /// Indicates whether throughput is configured at the database level (shared across containers).
    /// </summary>
    bool IsDatabaseShared();

    /// <summary>
    /// Indicates whether throughput is configured at the container level (dedicated per container).
    /// </summary>
    bool IsContainerDedicated();
}
