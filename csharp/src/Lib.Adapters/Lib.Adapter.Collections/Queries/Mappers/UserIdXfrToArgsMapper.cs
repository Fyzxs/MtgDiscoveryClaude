using System.Threading.Tasks;
using Lib.Adapter.Collections.Apis.Entities;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions.Entities;

namespace Lib.Adapter.Collections.Queries.Mappers;

internal sealed class UserIdXfrToArgsMapper : IUserIdXfrToArgsMapper
{
    public Task<UserIdExtEntity> Map(IUserIdXfrEntity source)
    {
        UserIdExtEntity args = new() { UserId = source.UserId };

        return Task.FromResult(args);
    }
}
