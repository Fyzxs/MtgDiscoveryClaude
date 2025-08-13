using Lib.Cosmos.Apis.Ids;

namespace Lib.Scryfall.Ingestion.Cosmos.Values;

internal sealed class MtgDiscoveryCosmosAccountName : CosmosFriendlyAccountName
{
    public override string AsSystemType() => "MtgDiscovery";
}
