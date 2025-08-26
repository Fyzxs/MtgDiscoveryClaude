using Lib.Adapter.Scryfall.Cosmos.Cosmos.Containers;
using Lib.Cosmos.Apis;
using Lib.Cosmos.Apis.Operators;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.Operators;

public sealed class ScryfallCardsByNameScribe : CosmosScribe
{
    public ScryfallCardsByNameScribe(ILogger logger) : base(new CardsByNameCosmosContainer(logger))
    {
    }
}