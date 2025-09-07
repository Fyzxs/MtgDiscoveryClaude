using Lib.Cosmos.Apis.Ids;

namespace Lib.Adapter.Scryfall.Cosmos.Cosmos.Primitives;

internal sealed class CardNameTrigramsCosmosContainerName : CosmosContainerName
{
    public override string AsSystemType() => "CardNameTrigrams";
}
