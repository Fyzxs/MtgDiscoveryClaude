using Lib.Cosmos.Apis.Operators;
using Lib.Scryfall.Ingestion.Cosmos.Containers;
using Microsoft.Extensions.Logging;

namespace Lib.Scryfall.Ingestion.Cosmos.Operators;

internal sealed class ScryfallCardItemsScribe : CosmosScribe
{
    public ScryfallCardItemsScribe(ILogger logger)
        : base(new CardItemsCosmosContainer(logger))
    {
    }
}