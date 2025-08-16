using Lib.Cosmos.Apis;

namespace Lib.Scryfall.Ingestion.Cosmos.Containers;

internal sealed class SetItemsCosmosContainer : CosmosContainerAdapter
{
    public SetItemsCosmosContainer(ILogger logger)
        : base(logger, new SetItemsCosmosContainerDefinition(), new ServiceLocatorAuthCosmosConnectionConfig())
    {
    }
}
