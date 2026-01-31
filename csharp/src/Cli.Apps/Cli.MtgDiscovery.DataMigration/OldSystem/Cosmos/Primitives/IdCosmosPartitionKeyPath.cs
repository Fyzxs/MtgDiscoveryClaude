using Lib.Cosmos.Apis.Ids;

namespace Cli.MtgDiscovery.DataMigration.OldSystem.Cosmos.Primitives;

internal sealed class IdCosmosPartitionKeyPath : CosmosPartitionKeyPath
{
    public override string AsSystemType() => "/id";
}
