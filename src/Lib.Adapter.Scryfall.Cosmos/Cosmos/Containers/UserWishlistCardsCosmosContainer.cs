using Lib.Adapter.Scryfall.Cosmos.Cosmos.Containers.Definitions;
using Lib.Cosmos.Apis;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.Scryfall.Cosmos.Cosmos.Containers;

internal sealed class UserWishlistCardsCosmosContainer : CosmosContainerAdapter
{
    public UserWishlistCardsCosmosContainer(ILogger logger)
        : base(logger, new UserWishlistCardsCosmosContainerDefinition(), new ServiceLocatorAuthCosmosConnectionConfig())
    { }
}
