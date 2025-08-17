using Lib.Cosmos.Apis.Ids;

namespace Lib.Adapter.Scryfall.Cosmos.Cosmos.Values;

internal sealed class MtgDiscoveryCosmosAccountName : CosmosFriendlyAccountName
{
    public override string AsSystemType() => "MtgDiscovery";
}
