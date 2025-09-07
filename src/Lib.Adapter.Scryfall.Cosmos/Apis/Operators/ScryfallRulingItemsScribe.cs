using Lib.Adapter.Scryfall.Cosmos.Cosmos.Containers;
using Lib.Cosmos.Apis.Operators;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.Operators;

public sealed class ScryfallRulingItemsScribe : CosmosScribe
{
    public ScryfallRulingItemsScribe(ILogger logger)
        : base(new RulingItemsCosmosContainer(logger))
    { }
}
