using System.Threading.Tasks;
using Lib.Adapter.Cards.Apis.Entities;
using Lib.Aggregator.Cards.Queries.Mappers;
using Lib.Shared.DataModels.Entities.Itrs;

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
