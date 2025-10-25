using Lib.Cosmos.Apis.Ids;

namespace Cli.MtgDiscovery.DataMigration.OldSystem.Cosmos.Primitives;

internal sealed class MtgDiscoveryOldCosmosDatabaseName : CosmosDatabaseName
{
    public override string AsSystemType() => "MtgDiscovery";
}
