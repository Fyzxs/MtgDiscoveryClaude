using System.Threading.Tasks;
using Lib.Aggregator.Cards.Queries.Mappers;

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
