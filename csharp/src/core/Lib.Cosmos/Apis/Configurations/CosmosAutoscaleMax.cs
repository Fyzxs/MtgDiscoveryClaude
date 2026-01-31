using Lib.Universal.Primitives;

namespace Lib.Cosmos.Apis.Configurations;

/// <summary>
/// Represents a strongly-typed Azure Cosmos DB autoscale maximum throughput.
/// Used for both database-level and container-level throughput configuration.
/// </summary>
public abstract class CosmosAutoscaleMax : ToSystemType<int>;
