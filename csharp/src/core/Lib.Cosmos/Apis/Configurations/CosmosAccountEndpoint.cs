using Lib.Universal.Primitives;

namespace Lib.Cosmos.Apis.Configurations;

/// <summary>
/// Represents a strongly-typed Azure Cosmos DB account endpoint URL.
/// </summary>
public abstract class CosmosAccountEndpoint : ToSystemType<string>;
