using Cli.MtgDiscovery.PriceUpdate.Cosmos.Definitions;
using Lib.Cosmos.Apis;
using Microsoft.Extensions.Logging;

namespace Cli.MtgDiscovery.PriceUpdate.Cosmos.Containers;

internal sealed class ArtistCardsCosmosContainer : CosmosContainerAdapter
{
    public ArtistCardsCosmosContainer(ILogger logger)
        : base(logger, new ArtistCardsCosmosContainerDefinition(), new ServiceLocatorAuthCosmosConnectionConfig())
    { }
}
