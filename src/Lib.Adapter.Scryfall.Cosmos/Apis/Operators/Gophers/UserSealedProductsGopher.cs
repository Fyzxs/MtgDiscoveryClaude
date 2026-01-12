using Lib.Adapter.Scryfall.Cosmos.Cosmos.Containers;
using Lib.Cosmos.Apis.Operators;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Gophers;

public sealed class UserSealedProductsGopher : CosmosGopher
{
    public UserSealedProductsGopher(ILogger logger)
        : base(new UserSealedProductsCosmosContainer(logger))
    { }
}
