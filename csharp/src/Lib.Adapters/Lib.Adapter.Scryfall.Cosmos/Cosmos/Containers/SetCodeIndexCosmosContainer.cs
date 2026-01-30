using Lib.Adapter.Scryfall.Cosmos.Cosmos.Containers.Definitions;
using Lib.Cosmos.Apis;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.Scryfall.Cosmos.Cosmos.Containers;

internal sealed class SetCodeIndexCosmosContainer : CosmosContainerAdapter
{
    public SetCodeIndexCosmosContainer(ILogger logger)
        : base(logger, new SetCodeIndexCosmosContainerDefinition(), new ServiceLocatorAuthCosmosConnectionConfig())
    { }
}
