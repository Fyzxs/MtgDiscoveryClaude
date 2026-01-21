using Lib.Cosmos.Apis.Ids;

namespace Cli.MtgDiscovery.PriceUpdate.Cosmos.Primitives;

internal sealed class CardsByNameCosmosContainerName : CosmosContainerName
{
    public override string AsSystemType() => "CardsByName";
}
