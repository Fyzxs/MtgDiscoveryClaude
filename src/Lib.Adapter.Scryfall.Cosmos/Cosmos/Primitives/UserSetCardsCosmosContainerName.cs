using Lib.Cosmos.Apis.Ids;

namespace Lib.Adapter.Scryfall.Cosmos.Cosmos.Primitives;

internal sealed class UserSetCardsCosmosContainerName : CosmosContainerName
{
    public override string AsSystemType() => "UserSetCards";
}
