using Lib.Cosmos.Apis;
using Microsoft.Extensions.Logging;

namespace Lib.Scryfall.Ingestion.Cosmos.Containers;

internal sealed class SetItemsCosmosContainer : CosmosContainerAdapter
{
    public SetItemsCosmosContainer(ILogger logger)
        : base(logger, new SetItemsCosmosContainerDefinition(), new ServiceLocatorAuthCosmosConnectionConfig())
    {
    }
}
