using Lib.Cosmos.Apis;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.Scryfall.Cosmos.Cosmos.Containers;

internal sealed class CardItemsCosmosContainer : CosmosContainerAdapter
{
    public CardItemsCosmosContainer(ILogger logger)
        : base(logger, new CardItemsCosmosContainerDefinition(), new ServiceLocatorAuthCosmosConnectionConfig())
    {
    }
}
