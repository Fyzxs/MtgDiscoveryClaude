using Lib.Cosmos.Apis;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.Scryfall.Cosmos.Cosmos.Containers;

internal sealed class ArtistNameTrigramsCosmosContainer : CosmosContainerAdapter
{
    public ArtistNameTrigramsCosmosContainer(ILogger logger)
        : base(logger, new ArtistNameTrigramsCosmosContainerDefinition(), new ServiceLocatorAuthCosmosConnectionConfig())
    {
    }
}