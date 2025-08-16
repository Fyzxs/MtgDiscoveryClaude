using Lib.Cosmos.Apis;
using Microsoft.Extensions.Logging;

namespace Lib.Scryfall.Ingestion.Cosmos.Containers;

internal sealed class SetAssociationsCosmosContainer : CosmosContainerAdapter, ISetAssociationsCosmosContainer
{
    public SetAssociationsCosmosContainer(ILogger logger)
        : base(logger, new SetAssociationsCosmosContainerDefinition(), new ServiceLocatorAuthCosmosConnectionConfig())
    {
    }
}
