using Lib.Cosmos.Apis;
using Microsoft.Extensions.Logging;

namespace Lib.Scryfall.Ingestion.Cosmos.Containers;

internal sealed class CardItemsCosmosContainer : CosmosContainerAdapter
{
    public CardItemsCosmosContainer(ILogger logger)
        : base(logger, new CardItemsCosmosContainerDefinition(), new ServiceLocatorAuthCosmosConnectionConfig())
    {
    }
}
