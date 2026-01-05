using Cli.MtgDiscovery.PriceUpdate.Cosmos.Definitions;
using Lib.Cosmos.Apis;
using Microsoft.Extensions.Logging;

namespace Cli.MtgDiscovery.PriceUpdate.Cosmos.Containers;

internal sealed class CardsByNameCosmosContainer : CosmosContainerAdapter
{
    public CardsByNameCosmosContainer(ILogger logger)
        : base(logger, new CardsByNameCosmosContainerDefinition(), new ServiceLocatorAuthCosmosConnectionConfig())
    { }
}
