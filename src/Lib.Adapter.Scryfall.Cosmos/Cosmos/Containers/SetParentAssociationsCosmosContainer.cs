using Lib.Cosmos.Apis;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.Scryfall.Cosmos.Cosmos.Containers;

internal sealed class SetParentAssociationsCosmosContainer : CosmosContainerAdapter
{
    public SetParentAssociationsCosmosContainer(ILogger logger)
        : base(logger, new SetParentAssociationsCosmosContainerDefinition(), new ServiceLocatorAuthCosmosConnectionConfig())
    {
    }
}
