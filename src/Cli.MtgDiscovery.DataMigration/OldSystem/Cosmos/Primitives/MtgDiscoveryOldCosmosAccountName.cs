using Lib.Cosmos.Apis.Ids;

namespace Cli.MtgDiscovery.DataMigration.OldSystem.Cosmos.Primitives;

internal sealed class MtgDiscoveryOldCosmosAccountName : CosmosFriendlyAccountName
{
    public override string AsSystemType() => "MtgDiscoveryOld";
}
