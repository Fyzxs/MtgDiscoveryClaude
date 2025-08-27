using Lib.Adapter.Scryfall.Cosmos.Cosmos.Values;
using Lib.Cosmos.Apis.Configurations;
using Lib.Cosmos.Apis.Ids;

namespace Lib.Adapter.Scryfall.Cosmos.Cosmos.Containers;

internal sealed class SetCodeIndexCosmosContainerDefinition : ICosmosContainerDefinition
{
    public CosmosFriendlyAccountName FriendlyAccountName() => new MtgDiscoveryCosmosAccountName();

    public CosmosDatabaseName DatabaseName() => new MtgDiscoveryCosmosDatabaseName();

    public CosmosContainerName ContainerName() => new SetCodeIndexCosmosContainerName();

    public CosmosPartitionKeyPath PartitionKeyPath() => new PartitionCosmosPartitionKeyPath();
}