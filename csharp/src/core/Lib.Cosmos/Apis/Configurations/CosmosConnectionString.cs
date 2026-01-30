using Lib.Universal.Primitives;

namespace Lib.Cosmos.Apis.Configurations;

/// <summary>
/// Represents a strongly-typed Azure Cosmos DB connection string.
/// </summary>
public abstract class CosmosConnectionString : ToSystemType<string>;
