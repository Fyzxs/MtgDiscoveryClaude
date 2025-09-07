using Lib.Adapter.Scryfall.Cosmos.Cosmos.Containers.Definitions;
using Lib.Cosmos.Apis;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.Scryfall.Cosmos.Cosmos.Containers;

internal sealed class ArtistCardsCosmosContainer : CosmosContainerAdapter
{
    public ArtistCardsCosmosContainer(ILogger logger)
        : base(logger, new ArtistCardsCosmosContainerDefinition(), new ServiceLocatorAuthCosmosConnectionConfig())
    { }
}
