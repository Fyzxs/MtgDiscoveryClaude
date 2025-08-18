using Lib.Cosmos.Apis.Ids;

namespace Lib.Adapter.Scryfall.Cosmos.Cosmos.Values;

internal sealed class ArtistItemsCosmosContainerName : CosmosContainerName
{
    public override string AsSystemType() => "ArtistItems";
}
