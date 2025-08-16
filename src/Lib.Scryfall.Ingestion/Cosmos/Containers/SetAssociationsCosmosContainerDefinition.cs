using Lib.Cosmos.Apis.Ids;
using Lib.Scryfall.Ingestion.Cosmos.Values;

namespace Lib.Scryfall.Ingestion.Cosmos.Containers;

internal sealed class SetAssociationsCosmosContainerDefinition : ISetAssociationsCosmosContainerDefinition
{
    public CosmosFriendlyAccountName FriendlyAccountName() => new MtgDiscoveryCosmosAccountName();

    public CosmosDatabaseName DatabaseName() => new MtgDiscoveryCosmosDatabaseName();

    public CosmosContainerName ContainerName() => new SetAssociationsCosmosContainerName();

    public CosmosPartitionKeyPath PartitionKeyPath() => new PartitionCosmosPartitionKeyPath();
}
