using Lib.Adapter.Scryfall.Cosmos.Cosmos.Containers.Definitions;
using Lib.Cosmos.Apis;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.Scryfall.Cosmos.Cosmos.Containers;

internal sealed class RulingItemsCosmosContainer : CosmosContainerAdapter
{
    public RulingItemsCosmosContainer(ILogger logger)
        : base(logger, new RulingItemsCosmosContainerDefinition(), new ServiceLocatorAuthCosmosConnectionConfig())
    { }
}
