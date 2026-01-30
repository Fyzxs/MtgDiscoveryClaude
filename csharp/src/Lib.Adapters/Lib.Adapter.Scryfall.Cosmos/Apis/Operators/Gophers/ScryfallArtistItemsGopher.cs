using Lib.Adapter.Scryfall.Cosmos.Cosmos.Containers;
using Lib.Cosmos.Apis.Operators;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Gophers;

public sealed class ScryfallArtistItemsGopher : CosmosGopher
{
    public ScryfallArtistItemsGopher(ILogger logger)
        : base(new ArtistItemsCosmosContainer(logger))
    { }
}
