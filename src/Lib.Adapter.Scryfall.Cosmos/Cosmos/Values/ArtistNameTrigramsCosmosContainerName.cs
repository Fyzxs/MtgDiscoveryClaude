using Lib.Cosmos.Apis.Ids;

namespace Lib.Adapter.Scryfall.Cosmos.Cosmos.Values;

internal sealed class ArtistNameTrigramsCosmosContainerName : CosmosContainerName
{
    public override string AsSystemType() => "ArtistNameTrigrams";
}