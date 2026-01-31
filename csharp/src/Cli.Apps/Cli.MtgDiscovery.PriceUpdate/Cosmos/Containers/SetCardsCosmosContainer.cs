using Cli.MtgDiscovery.PriceUpdate.Cosmos.Definitions;
using Lib.Cosmos.Apis;
using Microsoft.Extensions.Logging;

namespace Cli.MtgDiscovery.PriceUpdate.Cosmos.Containers;

internal sealed class SetCardsCosmosContainer : CosmosContainerAdapter
{
    public SetCardsCosmosContainer(ILogger logger)
        : base(logger, new SetCardsCosmosContainerDefinition(), new ServiceLocatorAuthCosmosConnectionConfig())
    { }
}
