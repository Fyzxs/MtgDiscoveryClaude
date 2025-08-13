using Lib.Cosmos.Apis.Ids;

namespace Lib.Scryfall.Ingestion.Cosmos.Values;

internal sealed class SetItemsCosmosContainerName : CosmosContainerName
{
    public override string AsSystemType() => "SetItems";
}
