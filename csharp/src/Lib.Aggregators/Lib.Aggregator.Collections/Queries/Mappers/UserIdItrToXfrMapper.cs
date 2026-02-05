using System.Threading.Tasks;
using Lib.Adapter.Collections.Apis.Entities;
using Lib.Aggregator.Collections.Queries.Entities;
using Lib.Shared.DataModels.Entities.Itrs.User;

namespace Lib.Aggregator.Collections.Queries.Mappers;

internal sealed class UserIdItrToXfrMapper : IUserIdItrToXfrMapper
{
    public Task<IUserIdXfrEntity> Map(IUserIdItrEntity source)
    {
        IUserIdXfrEntity result = new UserIdXfrEntity { UserId = source.UserId };
        return Task.FromResult(result);
    }
}
