using Lib.Cosmos.Apis;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.Scryfall.Cosmos.Cosmos.Containers;

internal sealed class ArtistItemsCosmosContainer : CosmosContainerAdapter
{
    public ArtistItemsCosmosContainer(ILogger logger)
        : base(logger, new ArtistItemsCosmosContainerDefinition(), new ServiceLocatorAuthCosmosConnectionConfig())
    {
    }
}
