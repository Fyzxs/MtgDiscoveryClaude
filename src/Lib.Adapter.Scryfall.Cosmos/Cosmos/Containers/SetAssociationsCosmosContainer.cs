using Lib.Cosmos.Apis;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.Scryfall.Cosmos.Cosmos.Containers;

internal sealed class SetAssociationsCosmosContainer : CosmosContainerAdapter
{
    public SetAssociationsCosmosContainer(ILogger logger)
        : base(logger, new SetAssociationsCosmosContainerDefinition(), new ServiceLocatorAuthCosmosConnectionConfig())
    {
    }
}
