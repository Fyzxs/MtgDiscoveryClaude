using Lib.Adapter.Scryfall.Cosmos.Cosmos.Containers;
using Lib.Cosmos.Apis.Operators;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.Operators;

public sealed class CardNameTrigramsScribe : CosmosScribe
{
    public CardNameTrigramsScribe(ILogger logger)
        : base(new CardNameTrigramsCosmosContainer(logger))
    {
    }
}