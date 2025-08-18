using Lib.Cosmos.Apis.Ids;

namespace Lib.Adapter.Scryfall.Cosmos.Cosmos.Values;

internal sealed class ArtistSetsCosmosContainerName : CosmosContainerName
{
    public override string AsSystemType() => "ArtistSets";
}
