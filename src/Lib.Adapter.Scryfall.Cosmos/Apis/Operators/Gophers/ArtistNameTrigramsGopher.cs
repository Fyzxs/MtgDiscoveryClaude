using Lib.Adapter.Scryfall.Cosmos.Cosmos.Containers;
using Lib.Cosmos.Apis.Operators;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Gophers;

public sealed class ArtistNameTrigramsGopher : CosmosGopher
{
    public ArtistNameTrigramsGopher(ILogger logger)
        : base(new ArtistNameTrigramsCosmosContainer(logger))
    { }
}