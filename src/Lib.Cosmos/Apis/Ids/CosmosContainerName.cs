using Lib.Universal.Primitives;

namespace Lib.Cosmos.Apis.Ids;

/// <summary>
/// Represents a strongly-typed Azure Cosmos DB container name.
/// </summary>
public abstract class CosmosContainerName : ToSystemType<string>;
