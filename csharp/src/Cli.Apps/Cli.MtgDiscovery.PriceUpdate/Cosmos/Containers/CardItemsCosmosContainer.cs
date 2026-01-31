using Cli.MtgDiscovery.PriceUpdate.Cosmos.Definitions;
using Lib.Cosmos.Apis;
using Microsoft.Extensions.Logging;

namespace Cli.MtgDiscovery.PriceUpdate.Cosmos.Containers;

internal sealed class CardItemsCosmosContainer : CosmosContainerAdapter
{
    public CardItemsCosmosContainer(ILogger logger)
        : base(logger, new CardItemsCosmosContainerDefinition(), new ServiceLocatorAuthCosmosConnectionConfig())
    { }
}
