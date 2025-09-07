using Lib.Cosmos.Apis.Ids;

namespace Lib.Adapter.Scryfall.Cosmos.Cosmos.Primitives;

internal sealed class ArtistCardsCosmosContainerName : CosmosContainerName
{
    public override string AsSystemType() => "ArtistCards";
}
