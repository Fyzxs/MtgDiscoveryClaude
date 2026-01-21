using System.Threading.Tasks;
using Lib.Adapter.Cards.Apis.Entities;
using Lib.Aggregator.Cards.Queries.Mappers;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.Aggregator.Cards.Tests.Fakes;

internal sealed class SetCodeItrToXfrMapperFake : ISetCodeItrToXfrMapper
{
    public ISetCodeXfrEntity MapResult { get; init; } = default!;
    public int MapInvokeCount { get; private set; }
    public ISetCodeItrEntity MapInput { get; private set; } = default!;

    public Task<ISetCodeXfrEntity> Map(ISetCodeItrEntity source)
    {
        MapInvokeCount++;
        MapInput = source;
        return Task.FromResult(MapResult);
    }
}
