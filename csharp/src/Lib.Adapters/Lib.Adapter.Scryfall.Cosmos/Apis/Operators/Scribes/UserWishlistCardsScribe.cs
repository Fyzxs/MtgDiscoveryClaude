using Lib.Adapter.Scryfall.Cosmos.Cosmos.Containers;
using Lib.Cosmos.Apis.Operators;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Scribes;

public sealed class UserWishlistCardsScribe : CosmosScribe
{
    public UserWishlistCardsScribe(ILogger logger)
        : base(new UserWishlistCardsCosmosContainer(logger))
    { }
}
