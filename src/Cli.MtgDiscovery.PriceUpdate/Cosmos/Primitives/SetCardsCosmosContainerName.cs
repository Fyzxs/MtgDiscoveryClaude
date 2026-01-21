using Lib.Cosmos.Apis.Ids;

namespace Cli.MtgDiscovery.PriceUpdate.Cosmos.Primitives;

internal sealed class SetCardsCosmosContainerName : CosmosContainerName
{
    public override string AsSystemType() => "SetCards";
}
