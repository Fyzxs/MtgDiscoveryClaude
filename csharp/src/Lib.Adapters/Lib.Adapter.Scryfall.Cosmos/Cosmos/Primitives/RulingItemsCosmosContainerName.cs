using Lib.Cosmos.Apis.Ids;

namespace Lib.Adapter.Scryfall.Cosmos.Cosmos.Primitives;

internal sealed class RulingItemsCosmosContainerName : CosmosContainerName
{
    public override string AsSystemType() => "RulingItems";
}
