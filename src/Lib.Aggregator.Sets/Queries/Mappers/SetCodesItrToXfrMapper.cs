using System.Threading.Tasks;
using Lib.Adapter.Sets.Apis.Entities;
using Lib.Aggregator.Sets.Queries.Entities;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.Aggregator.Sets.Queries.Mappers;

internal sealed class SetCodesItrToXfrMapper : ISetCodesItrToXfrMapper
{
    public Task<ISetCodesXfrEntity> Map(ISetCodesItrEntity source)
    {
        return Task.FromResult<ISetCodesXfrEntity>(new SetCodesXfrEntity
        {
            SetCodes = source.SetCodes
        });
    }
}