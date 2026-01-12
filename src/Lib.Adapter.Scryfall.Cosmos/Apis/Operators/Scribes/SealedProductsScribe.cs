using Lib.Adapter.Scryfall.Cosmos.Cosmos.Containers;
using Lib.Cosmos.Apis.Operators;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Scribes;

public sealed class SealedProductsScribe : CosmosScribe
{
    public SealedProductsScribe(ILogger logger)
        : base(new SealedProductsCosmosContainer(logger))
    { }
}
