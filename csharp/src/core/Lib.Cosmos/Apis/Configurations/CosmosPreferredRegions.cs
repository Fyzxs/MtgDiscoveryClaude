using System.Collections.Generic;
using Lib.Universal.Primitives;

namespace Lib.Cosmos.Apis.Configurations;

/// <summary>
/// Represents a strongly-typed list of preferred Azure regions for Cosmos DB connection.
/// </summary>
public abstract class CosmosPreferredRegions : ToSystemType<IReadOnlyList<string>>;
