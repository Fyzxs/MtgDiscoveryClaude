using Lib.Cosmos.Apis.Ids;

namespace Cli.MtgDiscovery.PriceUpdate.Cosmos.Primitives;

internal sealed class MtgDiscoveryCosmosDatabaseName : CosmosDatabaseName
{
    public override string AsSystemType() => "MtgDiscoveryV4";
}
