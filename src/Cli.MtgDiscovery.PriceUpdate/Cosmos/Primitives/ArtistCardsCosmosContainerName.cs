using Lib.Cosmos.Apis.Ids;

namespace Cli.MtgDiscovery.PriceUpdate.Cosmos.Primitives;

internal sealed class ArtistCardsCosmosContainerName : CosmosContainerName
{
    public override string AsSystemType() => "ArtistCards";
}
