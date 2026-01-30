using Lib.Universal.Primitives;
using Microsoft.Azure.Cosmos;

namespace Lib.Cosmos.Apis.Ids;

/// <summary>
/// Represents a strongly-typed Azure Cosmos DB partition key value.
/// </summary>
public abstract class PartitionKeyValue : ToSystemType<PartitionKey>;
