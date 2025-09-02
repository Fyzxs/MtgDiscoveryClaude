using Lib.Adapter.Scryfall.Cosmos.Cosmos.Values;
using Lib.Cosmos.Apis.Configurations;
using Lib.Cosmos.Apis.Ids;

namespace Lib.Adapter.Scryfall.Cosmos.Cosmos.Containers;

internal sealed class RulingItemsCosmosContainerDefinition : ICosmosContainerDefinition
{
    public CosmosFriendlyAccountName FriendlyAccountName() => new MtgDiscoveryCosmosAccountName();

    public CosmosDatabaseName DatabaseName() => new MtgDiscoveryCosmosDatabaseName();

    public CosmosContainerName ContainerName() => new RulingItemsCosmosContainerName();

    public CosmosPartitionKeyPath PartitionKeyPath() => new PartitionCosmosPartitionKeyPath();
}