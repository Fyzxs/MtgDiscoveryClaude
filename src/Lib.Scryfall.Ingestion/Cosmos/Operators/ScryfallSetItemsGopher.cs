using Lib.Cosmos.Apis.Operators;
using Lib.Scryfall.Ingestion.Cosmos.Containers;
using Microsoft.Extensions.Logging;

namespace Lib.Scryfall.Ingestion.Cosmos.Operators;

internal sealed class ScryfallSetItemsGopher : CosmosGopher, IScryfallSetItemsGopher
{
    public ScryfallSetItemsGopher(ILogger logger)
        : base(new SetItemsCosmosContainer(logger))
    {
    }
}
