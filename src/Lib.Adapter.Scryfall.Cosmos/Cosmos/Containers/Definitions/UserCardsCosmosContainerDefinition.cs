using Lib.Adapter.Scryfall.Cosmos.Cosmos.Primitives;
using Lib.Cosmos.Apis.Configurations;
using Lib.Cosmos.Apis.Ids;

namespace Lib.Adapter.Scryfall.Cosmos.Cosmos.Containers.Definitions;

internal sealed class UserCardsCosmosContainerDefinition : ICosmosContainerDefinition
{
    public CosmosFriendlyAccountName FriendlyAccountName() => new MtgDiscoveryCosmosAccountName();
    public CosmosDatabaseName DatabaseName() => new MtgDiscoveryCosmosDatabaseName();
    public CosmosContainerName ContainerName() => new UserCardsCosmosContainerName();
    public CosmosPartitionKeyPath PartitionKeyPath() => new PartitionCosmosPartitionKeyPath();
}
