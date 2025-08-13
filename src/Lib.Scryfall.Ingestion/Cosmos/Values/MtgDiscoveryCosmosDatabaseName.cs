using Lib.Cosmos.Apis.Ids;

namespace Lib.Scryfall.Ingestion.Cosmos.Values;

internal sealed class MtgDiscoveryCosmosDatabaseName : CosmosDatabaseName
{
    public override string AsSystemType() => "MtgDiscoveryV4";
}
