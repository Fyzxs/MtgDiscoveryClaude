using Cli.MtgDiscovery.DataMigration.OldSystem.Cosmos.Containers.Definitions;
using Lib.Cosmos.Apis;
using Microsoft.Extensions.Logging;

namespace Cli.MtgDiscovery.DataMigration.OldSystem.Cosmos.Containers;

internal sealed class OldDiscoveryCardsCosmosContainer : CosmosContainerAdapter
{
    public OldDiscoveryCardsCosmosContainer(ILogger logger)
        : base(logger, new OldDiscoveryCardsCosmosContainerDefinition(), new ServiceLocatorAuthCosmosConnectionConfig())
    {
    }
}
