using Lib.Adapter.Scryfall.Cosmos.Cosmos.Containers;
using Lib.Cosmos.Apis.Operators;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Gophers;

public sealed class SealedProductsGopher : CosmosGopher
{
    public SealedProductsGopher(ILogger logger)
        : base(new SealedProductsCosmosContainer(logger))
    { }
}
