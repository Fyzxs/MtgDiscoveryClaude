using Lib.Adapter.Scryfall.Cosmos.Cosmos.Containers;
using Lib.Cosmos.Apis.Operators;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Janitors;

public sealed class UserSealedProductsJanitor : CosmosJanitor
{
    public UserSealedProductsJanitor(ILogger logger)
        : base(new UserSealedProductsCosmosContainer(logger))
    { }
}
