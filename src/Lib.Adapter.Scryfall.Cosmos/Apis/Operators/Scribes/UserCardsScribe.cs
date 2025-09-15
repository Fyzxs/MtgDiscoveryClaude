using Lib.Adapter.Scryfall.Cosmos.Apis.Operators;
using Lib.Adapter.Scryfall.Cosmos.Cosmos.Containers;
using Lib.Cosmos.Apis.Operators;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Scribes;

public sealed class UserCardsScribe : CosmosScribe
{
    public UserCardsScribe(ILogger logger)
        : base(new UserCardsCosmosContainer(logger))
    { }
}
