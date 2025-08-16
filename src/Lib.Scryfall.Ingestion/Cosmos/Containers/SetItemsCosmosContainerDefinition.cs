using Lib.Cosmos.Apis.Configurations;
using Lib.Cosmos.Apis.Ids;
using Lib.Scryfall.Ingestion.Cosmos.Values;

namespace Lib.Scryfall.Ingestion.Cosmos.Containers;

internal sealed class SetItemsCosmosContainerDefinition : ICosmosContainerDefinition
{
    public CosmosFriendlyAccountName FriendlyAccountName() => new MtgDiscoveryCosmosAccountName();

    public CosmosDatabaseName DatabaseName() => new MtgDiscoveryCosmosDatabaseName();

    public CosmosContainerName ContainerName() => new SetItemsCosmosContainerName();

    public CosmosPartitionKeyPath PartitionKeyPath() => new PartitionCosmosPartitionKeyPath();
}
