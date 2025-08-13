using Lib.Cosmos.Apis.Operators;
using Lib.Scryfall.Ingestion.Cosmos.Containers;
using Microsoft.Extensions.Logging;

namespace Lib.Scryfall.Ingestion.Cosmos.Operators;

internal sealed class ScryfallSetItemsScribe : CosmosScribe, IScryfallSetItemsScribe
{
    public ScryfallSetItemsScribe(ILogger logger)
        : base(new SetItemsCosmosContainer(logger))
    {
    }
}
