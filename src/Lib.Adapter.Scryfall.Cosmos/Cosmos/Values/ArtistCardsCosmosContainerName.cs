using Lib.Cosmos.Apis.Ids;

namespace Lib.Adapter.Scryfall.Cosmos.Cosmos.Values;

internal sealed class ArtistCardsCosmosContainerName : CosmosContainerName
{
    public override string AsSystemType() => "ArtistCards";
}