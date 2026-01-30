using Lib.Cosmos.Apis.Ids;

namespace Lib.Adapter.Scryfall.Cosmos.Cosmos.Primitives;

internal sealed class CardsByNameCosmosContainerName : CosmosContainerName
{
    public override string AsSystemType() => "CardsByName";
}
