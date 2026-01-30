using Lib.Adapter.Scryfall.Cosmos.Cosmos.Containers.Definitions;
using Lib.Cosmos.Apis;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.Scryfall.Cosmos.Cosmos.Containers;

internal sealed class UserInfoCosmosContainer : CosmosContainerAdapter
{
    public UserInfoCosmosContainer(ILogger logger)
        : base(logger, new UserInfoCosmosContainerDefinition(), new ServiceLocatorAuthCosmosConnectionConfig())
    { }
}
