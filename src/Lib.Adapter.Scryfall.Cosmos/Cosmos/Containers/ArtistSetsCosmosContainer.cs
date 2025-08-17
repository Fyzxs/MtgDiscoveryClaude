using Lib.Cosmos.Apis;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.Scryfall.Cosmos.Cosmos.Containers;

internal sealed class ArtistSetsCosmosContainer : CosmosContainerAdapter
{
    public ArtistSetsCosmosContainer(ILogger logger)
        : base(logger, new ArtistSetsCosmosContainerDefinition(), new ServiceLocatorAuthCosmosConnectionConfig())
    {
    }
}