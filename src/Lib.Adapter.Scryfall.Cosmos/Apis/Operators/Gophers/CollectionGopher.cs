using Lib.Adapter.Scryfall.Cosmos.Cosmos.Containers;
using Lib.Cosmos.Apis.Operators;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Gophers;

public sealed class CollectionGopher : CosmosGopher
{
    public CollectionGopher(ILogger logger) : base(new CollectionsCosmosContainer(logger))
    { }
}
