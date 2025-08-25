using Lib.Cosmos.Apis.Ids;

namespace Lib.Adapter.Scryfall.Cosmos.Cosmos.Values;

internal sealed class SetCodeIndexCosmosContainerName : CosmosContainerName
{
    public override string AsSystemType() => "SetCodeIndex";
}