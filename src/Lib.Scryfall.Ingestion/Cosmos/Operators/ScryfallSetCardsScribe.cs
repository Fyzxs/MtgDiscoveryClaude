using Lib.Cosmos.Apis.Operators;
using Lib.Scryfall.Ingestion.Cosmos.Containers;
using Microsoft.Extensions.Logging;

namespace Lib.Scryfall.Ingestion.Cosmos.Operators;

internal sealed class ScryfallSetCardsScribe : CosmosScribe
{
    public ScryfallSetCardsScribe(ILogger logger)
        : base(new SetCardsCosmosContainer(logger))
    {
    }
}