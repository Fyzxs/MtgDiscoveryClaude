using Lib.Adapter.Scryfall.Cosmos.Cosmos.Containers.Definitions;
using Lib.Cosmos.Apis;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.Scryfall.Cosmos.Cosmos.Containers;

internal sealed class UserSetCardsCosmosContainer : CosmosContainerAdapter
{
    public UserSetCardsCosmosContainer(ILogger logger)
        : base(logger, new UserSetCardsCosmosContainerDefinition(), new ServiceLocatorAuthCosmosConnectionConfig())
    { }
}