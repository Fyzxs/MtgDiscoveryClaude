using System.Threading.Tasks;
using Lib.Adapter.Sets.Apis.Entities;
using Lib.Aggregator.Sets.Queries.Entities;
using Lib.Shared.DataModels.Entities;

namespace Lib.Aggregator.Sets.Queries.Mappers;

internal sealed class SetIdsItrToXfrMapper : ISetIdsItrToXfrMapper
{
    public Task<ISetIdsXfrEntity> Map(ISetIdsItrEntity source)
    {
        return Task.FromResult<ISetIdsXfrEntity>(new SetIdsXfrEntity
        {
            SetIds = source.SetIds
        });
    }
}