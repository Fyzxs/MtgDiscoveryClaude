using Lib.Cosmos.Apis.Ids;

namespace Cli.MtgDiscovery.DataMigration.OldSystem.Cosmos.Primitives;

internal sealed class OldDiscoveryCardsCosmosContainerName : CosmosContainerName
{
    public override string AsSystemType() => "DiscoveryCards";
}
