using System.Threading.Tasks;
using Lib.Adapter.Cards.Apis.Entities;
using Lib.Aggregator.Cards.Queries.Mappers;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.Aggregator.Cards.Tests.Fakes;

internal sealed class CardNameItrToXfrMapperFake : ICardNameItrToXfrMapper
{
    public ICardNameXfrEntity MapResult { get; init; } = default!;
    public int MapInvokeCount { get; private set; }
    public ICardNameItrEntity MapInput { get; private set; } = default!;

    public Task<ICardNameXfrEntity> Map(ICardNameItrEntity source)
    {
        MapInvokeCount++;
        MapInput = source;
        return Task.FromResult(MapResult);
    }
}
