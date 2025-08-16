using Lib.Cosmos.Apis.Configurations;
using Lib.Cosmos.Apis.Ids;
using Lib.Scryfall.Ingestion.Cosmos.Values;

namespace Lib.Scryfall.Ingestion.Cosmos.Containers;

internal sealed class SetCardsCosmosContainerDefinition : ICosmosContainerDefinition
{
    public CosmosFriendlyAccountName FriendlyAccountName() => new MtgDiscoveryCosmosAccountName();

    public CosmosDatabaseName DatabaseName() => new MtgDiscoveryCosmosDatabaseName();

    public CosmosContainerName ContainerName() => new SetCardsCosmosContainerName();

    public CosmosPartitionKeyPath PartitionKeyPath() => new PartitionCosmosPartitionKeyPath();
}