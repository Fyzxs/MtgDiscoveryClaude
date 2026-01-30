using Lib.Universal.Primitives;
using Microsoft.Azure.Cosmos;

namespace Lib.Cosmos.Apis.Configurations;

/// <summary>
/// Represents a strongly-typed Azure Cosmos DB connection mode.
/// </summary>
public abstract class CosmosConnectionMode : ToSystemType<ConnectionMode>;
