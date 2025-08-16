using Lib.Cosmos.Apis;
using Microsoft.Extensions.Logging;

namespace Lib.Scryfall.Ingestion.Cosmos.Containers;

internal sealed class SetCardsCosmosContainer : CosmosContainerAdapter
{
    public SetCardsCosmosContainer(ILogger logger)
        : base(logger, new SetCardsCosmosContainerDefinition(), new ServiceLocatorAuthCosmosConnectionConfig())
    {
    }
}