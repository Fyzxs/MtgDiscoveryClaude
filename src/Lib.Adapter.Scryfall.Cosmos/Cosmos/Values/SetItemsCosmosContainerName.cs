using Lib.Cosmos.Apis.Ids;

namespace Lib.Adapter.Scryfall.Cosmos.Cosmos.Values;

internal sealed class SetItemsCosmosContainerName : CosmosContainerName
{
    public override string AsSystemType() => "SetItems";
}
