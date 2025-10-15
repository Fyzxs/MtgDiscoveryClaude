using System.Threading.Tasks;
using Lib.Aggregator.Cards.Queries.Mappers;

namespace Lib.Aggregator.Cards.Tests.Fakes;

internal sealed class CardIdsItrToXfrMapperFake : ICardIdsItrToXfrMapper
{
    public ICardIdsXfrEntity MapResult { get; init; } = default!;
    public int MapInvokeCount { get; private set; }
    public ICardIdsItrEntity MapInput { get; private set; } = default!;

    public Task<ICardIdsXfrEntity> Map(ICardIdsItrEntity source)
    {
        MapInvokeCount++;
        MapInput = source;
        return Task.FromResult(MapResult);
    }
}
