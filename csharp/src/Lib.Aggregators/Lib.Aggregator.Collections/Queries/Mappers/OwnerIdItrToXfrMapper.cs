using System.Threading.Tasks;
using Lib.Adapter.Collections.Apis.Entities;
using Lib.Aggregator.Collections.Queries.Entities;
using Lib.Shared.DataModels.Entities.Itrs.Collections;

namespace Lib.Aggregator.Collections.Queries.Mappers;

internal sealed class OwnerIdItrToXfrMapper : IOwnerIdItrToXfrMapper
{
    public Task<IOwnerIdXfrEntity> Map(IOwnerIdItrEntity source)
    {
        IOwnerIdXfrEntity result = new OwnerIdXfrEntity { OwnerId = source.OwnerId };
        return Task.FromResult(result);
    }
}
