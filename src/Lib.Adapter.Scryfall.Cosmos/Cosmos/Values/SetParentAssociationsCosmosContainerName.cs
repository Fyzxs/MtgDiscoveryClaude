using Lib.Cosmos.Apis.Ids;

namespace Lib.Adapter.Scryfall.Cosmos.Cosmos.Values;

internal sealed class SetParentAssociationsCosmosContainerName : CosmosContainerName
{
    public override string AsSystemType() => "SetParentAssociations";
}
