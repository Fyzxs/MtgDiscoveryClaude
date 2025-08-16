using Lib.Cosmos.Apis;

namespace Lib.Scryfall.Ingestion.Cosmos.Containers;

internal sealed class SetAssociationsCosmosContainer : CosmosContainerAdapter
{
    public SetAssociationsCosmosContainer(ILogger logger)
        : base(logger, new SetAssociationsCosmosContainerDefinition(), new ServiceLocatorAuthCosmosConnectionConfig())
    {
    }
}
