using Lib.Cosmos.Apis.Ids;

namespace Lib.Scryfall.Ingestion.Cosmos.Values;

internal sealed class SetCardsCosmosContainerName : CosmosContainerName
{
    public override string AsSystemType() => "SetCards";
}