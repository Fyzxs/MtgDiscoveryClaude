using Lib.Adapter.Scryfall.Cosmos.Cosmos.Containers.Definitions;
using Lib.Cosmos.Apis;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.Scryfall.Cosmos.Cosmos.Containers;

internal sealed class CardNameTrigramsCosmosContainer : CosmosContainerAdapter
{
    public CardNameTrigramsCosmosContainer(ILogger logger)
        : base(logger, new CardNameTrigramsCosmosContainerDefinition(), new ServiceLocatorAuthCosmosConnectionConfig())
    { }
}
