using Lib.Cosmos.Apis.Ids;

namespace Lib.Adapter.Scryfall.Cosmos.Cosmos.Values;

internal sealed class CardsByNameCosmosContainerName : CosmosContainerName
{
    public override string AsSystemType() => "CardsByName";
}