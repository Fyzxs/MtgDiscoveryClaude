using Lib.Cosmos.Apis.Ids;

namespace Cli.MtgDiscovery.PriceUpdate.Cosmos.Primitives;

internal sealed class CardItemsCosmosContainerName : CosmosContainerName
{
    public override string AsSystemType() => "CardItems";
}
