#nullable enable
using System.Threading.Tasks;
using Lib.Adapter.Collections.Apis.Entities;
using Lib.Adapter.Collections.Queries.Entities;
using Lib.Adapter.Collections.Queries.Mappers;

namespace Lib.Adapter.Collections.Tests.Fakes;

public sealed class OwnerIdXfrToUserIdXfrMapperFake : IOwnerIdXfrToUserIdXfrMapper
{
    public int MapInvokeCount { get; private set; }

    public IOwnerIdXfrEntity? LastMapInput { get; private set; }

    public Task<IUserIdXfrEntity> Map(IOwnerIdXfrEntity source)
    {
        MapInvokeCount++;
        LastMapInput = source;

        IUserIdXfrEntity result = new UserIdXfrEntity { UserId = source.OwnerId };
        return Task.FromResult(result);
    }
}
