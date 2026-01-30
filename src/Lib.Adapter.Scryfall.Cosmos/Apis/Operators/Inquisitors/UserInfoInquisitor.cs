using Lib.Adapter.Scryfall.Cosmos.Cosmos.Containers;
using Lib.Cosmos.Apis.Operators;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitors;

public sealed class UserInfoInquisitor : CosmosInquisitor
{
    public UserInfoInquisitor(ILogger logger)
        : base(new UserInfoCosmosContainer(logger))
    { }
}
