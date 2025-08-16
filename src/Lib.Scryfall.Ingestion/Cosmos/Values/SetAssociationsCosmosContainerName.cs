using Lib.Cosmos.Apis.Ids;

namespace Lib.Scryfall.Ingestion.Cosmos.Values;

internal sealed class SetAssociationsCosmosContainerName : CosmosContainerName
{
    public override string AsSystemType() => "SetAssociations";
}
