using Lib.Universal.Primitives;

namespace Lib.Cosmos.Apis.Configurations;

/// <summary>
/// Represents a strongly-typed Azure Cosmos DB container time-to-live (TTL) in seconds.
/// </summary>
public abstract class CosmosContainerTimeToLive : ToSystemType<int?>;
