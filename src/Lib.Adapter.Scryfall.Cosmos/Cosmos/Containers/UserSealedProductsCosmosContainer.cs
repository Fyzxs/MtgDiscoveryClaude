using Lib.Adapter.Scryfall.Cosmos.Cosmos.Containers.Definitions;
using Lib.Cosmos.Apis;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.Scryfall.Cosmos.Cosmos.Containers;

internal sealed class UserSealedProductsCosmosContainer : CosmosContainerAdapter
{
    public UserSealedProductsCosmosContainer(ILogger logger)
        : base(logger, new UserSealedProductsCosmosContainerDefinition(), new ServiceLocatorAuthCosmosConnectionConfig())
    { }
}
