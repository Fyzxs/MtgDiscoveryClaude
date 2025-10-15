using System.Threading.Tasks;
using Lib.Aggregator.Cards.Queries.Mappers;

namespace Lib.Aggregator.Cards.Tests.Fakes;

internal sealed class CardSearchTermItrToXfrMapperFake : ICardSearchTermItrToXfrMapper
{
    public ICardSearchTermXfrEntity MapResult { get; init; } = default!;
    public int MapInvokeCount { get; private set; }
    public ICardSearchTermItrEntity MapInput { get; private set; } = default!;

    public Task<ICardSearchTermXfrEntity> Map(ICardSearchTermItrEntity source)
    {
        MapInvokeCount++;
        MapInput = source;
        return Task.FromResult(MapResult);
    }
}
