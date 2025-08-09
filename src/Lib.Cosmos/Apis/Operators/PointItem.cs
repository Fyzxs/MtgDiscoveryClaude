using Lib.Cosmos.Apis.Ids;

namespace Lib.Cosmos.Apis.Operators;

/// <summary>
/// Represents an item identifier for point operations in Cosmos DB.
/// </summary>
public abstract class PointItem
{
    /// <summary>
    /// Gets the item identifier.
    /// </summary>
    public CosmosItemId Id { get; set; }

    /// <summary>
    /// Gets the partition key value.
    /// </summary>
    public PartitionKeyValue Partition { get; set; }
}
