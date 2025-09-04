using Lib.Cosmos.Apis.Ids;

namespace Lib.Adapter.Scryfall.Cosmos.Cosmos.Values;

internal sealed class UserInfoCosmosContainerName : CosmosContainerName
{
    public override string AsSystemType() => "UserInfo";
}