using Lib.Cosmos.Apis.Ids;

namespace Lib.Adapter.Scryfall.Cosmos.Cosmos.Values;

internal sealed class SetCodeToIdAssociationsCosmosContainerName : CosmosContainerName
{
    public override string AsSystemType() => "SetCodeToIdAssociations";
}
