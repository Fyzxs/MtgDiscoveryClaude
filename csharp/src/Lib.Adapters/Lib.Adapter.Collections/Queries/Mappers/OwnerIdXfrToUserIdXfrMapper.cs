using System.Threading.Tasks;
using Lib.Adapter.Collections.Apis.Entities;
using Lib.Adapter.Collections.Queries.Entities;

namespace Lib.Adapter.Collections.Queries.Mappers;

internal sealed class OwnerIdXfrToUserIdXfrMapper : IOwnerIdXfrToUserIdXfrMapper
{
    public Task<IUserIdXfrEntity> Map(IOwnerIdXfrEntity source)
    {
        IUserIdXfrEntity result = new UserIdXfrEntity { UserId = source.OwnerId };

        return Task.FromResult(result);
    }
}
