using Lib.Cosmos.Apis.Operators;
using Lib.Scryfall.Ingestion.Cosmos.Containers;
using Microsoft.Extensions.Logging;

namespace Lib.Scryfall.Ingestion.Cosmos.Operators;

internal sealed class ScryfallSetAssociationsScribe : CosmosScribe
{
    public ScryfallSetAssociationsScribe(ILogger logger)
        : base(new SetAssociationsCosmosContainer(logger))
    {
    }
}
