using Lib.Adapter.Scryfall.Cosmos.Cosmos.Containers;
using Lib.Cosmos.Apis.Operators;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Janitors;

public sealed class UserWishlistCardsJanitor : CosmosJanitor
{
    public UserWishlistCardsJanitor(ILogger logger)
        : base(new UserWishlistCardsCosmosContainer(logger))
    { }
}
