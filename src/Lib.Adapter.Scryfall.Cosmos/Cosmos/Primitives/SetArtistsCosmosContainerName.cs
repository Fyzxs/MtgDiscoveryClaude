using Lib.Cosmos.Apis.Ids;

namespace Lib.Adapter.Scryfall.Cosmos.Cosmos.Primitives;

internal sealed class SetArtistsCosmosContainerName : CosmosContainerName
{
    public override string AsSystemType() => "SetArtists";
}
