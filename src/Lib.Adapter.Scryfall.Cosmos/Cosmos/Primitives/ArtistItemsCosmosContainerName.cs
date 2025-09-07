using Lib.Cosmos.Apis.Ids;

namespace Lib.Adapter.Scryfall.Cosmos.Cosmos.Primitives;

internal sealed class ArtistItemsCosmosContainerName : CosmosContainerName
{
    public override string AsSystemType() => "ArtistItems";
}
