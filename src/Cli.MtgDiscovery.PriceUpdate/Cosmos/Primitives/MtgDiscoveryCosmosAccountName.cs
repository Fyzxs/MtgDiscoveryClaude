using Lib.Cosmos.Apis.Ids;

namespace Cli.MtgDiscovery.PriceUpdate.Cosmos.Primitives;

internal sealed class MtgDiscoveryCosmosAccountName : CosmosFriendlyAccountName
{
    public override string AsSystemType() => "MtgDiscovery";
}
