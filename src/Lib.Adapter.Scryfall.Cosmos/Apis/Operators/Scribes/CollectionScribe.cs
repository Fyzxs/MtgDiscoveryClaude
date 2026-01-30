using Lib.Adapter.Scryfall.Cosmos.Cosmos.Containers;
using Lib.Cosmos.Apis.Operators;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Scribes;

public sealed class CollectionScribe : CosmosScribe
{
    public CollectionScribe(ILogger logger)
        : base(new CollectionsCosmosContainer(logger))
    { }
}
