using Lib.Cosmos.Apis.Ids;

namespace Lib.Scryfall.Ingestion.Cosmos.Values;

internal sealed class CardItemsCosmosContainerName : CosmosContainerName
{
    public override string AsSystemType() => "CardItems";
}
