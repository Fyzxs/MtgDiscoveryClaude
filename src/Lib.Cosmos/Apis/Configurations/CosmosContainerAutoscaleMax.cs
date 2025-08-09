using Lib.Universal.Primitives;

namespace Lib.Cosmos.Apis.Configurations;

/// <summary>
/// Represents a strongly-typed Azure Cosmos DB container autoscale maximum throughput.
/// </summary>
public abstract class CosmosContainerAutoscaleMax : ToSystemType<int>;
