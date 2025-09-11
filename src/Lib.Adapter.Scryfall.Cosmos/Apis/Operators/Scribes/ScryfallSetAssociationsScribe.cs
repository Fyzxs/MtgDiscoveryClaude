using Lib.Adapter.Scryfall.Cosmos.Cosmos.Containers;
using Lib.Cosmos.Apis.Operators;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Scribes;

public sealed class ScryfallSetAssociationsScribe : CosmosScribe
{
    public ScryfallSetAssociationsScribe(ILogger logger)
        : base(new SetParentAssociationsCosmosContainer(logger))
    { }
}
