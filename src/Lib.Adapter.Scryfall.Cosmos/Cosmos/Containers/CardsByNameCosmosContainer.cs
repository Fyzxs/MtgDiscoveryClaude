using Lib.Adapter.Scryfall.Cosmos.Cosmos.Containers.Definitions;
using Lib.Cosmos.Apis;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.Scryfall.Cosmos.Cosmos.Containers;

internal sealed class CardsByNameCosmosContainer : CosmosContainerAdapter
{
    public CardsByNameCosmosContainer(ILogger logger)
        : base(logger, new CardsByNameCosmosContainerDefinition(), new ServiceLocatorAuthCosmosConnectionConfig())
    { }
}
