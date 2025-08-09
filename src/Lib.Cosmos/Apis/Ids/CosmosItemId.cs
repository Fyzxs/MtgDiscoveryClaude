using Lib.Universal.Primitives;

namespace Lib.Cosmos.Apis.Ids;

/// <summary>
/// Represents a strongly-typed Azure Cosmos DB item identifier.
/// </summary>
public abstract class CosmosItemId : ToSystemType<string>;
