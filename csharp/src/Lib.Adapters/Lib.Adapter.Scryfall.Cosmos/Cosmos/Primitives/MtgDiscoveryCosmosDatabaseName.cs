using Lib.Cosmos.Apis.Ids;

namespace Lib.Adapter.Scryfall.Cosmos.Cosmos.Primitives;

internal sealed class MtgDiscoveryCosmosDatabaseName : CosmosDatabaseName
{
    public override string AsSystemType() => "MtgDiscoveryV4";
}
