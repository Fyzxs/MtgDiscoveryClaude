using Lib.Universal.Primitives;

namespace Lib.Cosmos.Apis.Ids;

/// <summary>
/// Represents a strongly-typed Azure Cosmos DB partition key path.
/// </summary>
public abstract class CosmosPartitionKeyPath : ToSystemType<string>;
