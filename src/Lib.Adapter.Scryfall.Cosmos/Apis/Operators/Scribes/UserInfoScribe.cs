using Lib.Adapter.Scryfall.Cosmos.Cosmos.Containers;
using Lib.Cosmos.Apis.Operators;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Scribes;

public sealed class UserInfoScribe : CosmosScribe
{
    public UserInfoScribe(ILogger logger)
        : base(new UserInfoCosmosContainer(logger))
    { }
}
