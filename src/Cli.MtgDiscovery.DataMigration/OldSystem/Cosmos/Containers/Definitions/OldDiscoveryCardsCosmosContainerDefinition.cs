using Cli.MtgDiscovery.DataMigration.OldSystem.Cosmos.Primitives;
using Lib.Cosmos.Apis.Configurations;
using Lib.Cosmos.Apis.Ids;

namespace Cli.MtgDiscovery.DataMigration.OldSystem.Cosmos.Containers.Definitions;

internal sealed class OldDiscoveryCardsCosmosContainerDefinition : ICosmosContainerDefinition
{
    public CosmosFriendlyAccountName FriendlyAccountName() => new MtgDiscoveryOldCosmosAccountName();

    public CosmosDatabaseName DatabaseName() => new MtgDiscoveryOldCosmosDatabaseName();

    public CosmosContainerName ContainerName() => new OldDiscoveryCardsCosmosContainerName();

    public CosmosPartitionKeyPath PartitionKeyPath() => new IdCosmosPartitionKeyPath();
}
