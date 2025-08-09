using Lib.Universal.Primitives;

namespace Lib.Cosmos.Apis.Configurations;

/// <summary>
/// Represents a strongly-typed Azure Cosmos DB client authentication mode.
/// </summary>
public abstract class CosmosClientAuthMode : ToSystemType<CosmosAuthMode>;
