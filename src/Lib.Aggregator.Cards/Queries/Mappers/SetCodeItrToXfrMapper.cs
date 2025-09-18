using System.Threading.Tasks;
using Lib.Adapter.Cards.Apis.Entities;
using Lib.Aggregator.Cards.Queries.Entities;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.Aggregator.Cards.Queries.Mappers;

internal sealed class SetCodeItrToXfrMapper : ISetCodeItrToXfrMapper
{
    public Task<ISetCodeXfrEntity> Map(ISetCodeItrEntity source)
    {
        return Task.FromResult<ISetCodeXfrEntity>(new SetCodeXfrEntity
        {
            SetCode = source.SetCode
        });
    }
}