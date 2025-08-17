using Lib.Cosmos.Apis.Ids;

namespace Lib.Adapter.Scryfall.Cosmos.Cosmos.Values;

internal sealed class SetAssociationsCosmosContainerName : CosmosContainerName
{
    public override string AsSystemType() => "SetAssociations";
}
