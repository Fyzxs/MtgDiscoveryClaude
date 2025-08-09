using Lib.Universal.Primitives;

namespace Lib.Cosmos.Apis.Ids;

/// <summary>
/// Represents a strongly-typed Azure Cosmos DB account name.
/// </summary>
public abstract class CosmosAccountName : ToSystemType<string>;

/// <summary>
/// Represents a strongly-typed Azure Cosmos DB account friendly.
/// </summary>
public abstract class CosmosFriendlyAccountName : ToSystemType<string>;
