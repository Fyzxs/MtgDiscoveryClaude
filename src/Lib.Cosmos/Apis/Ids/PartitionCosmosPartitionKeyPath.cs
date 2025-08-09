namespace Lib.Cosmos.Apis.Ids;

/// <summary>
/// Implementation of CosmosPartitionKeyPath that returns "/partition".
/// </summary>
public sealed class PartitionCosmosPartitionKeyPath : CosmosPartitionKeyPath
{
    /// <summary>
    /// Returns the partition key path.
    /// </summary>
    /// <returns>"/partition"</returns>
    public override string AsSystemType() => "/partition";
}