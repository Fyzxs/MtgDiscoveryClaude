using Lib.Adapter.Cards.Apis.Entities;

namespace Lib.Aggregator.Cards.Queries.Entities;

internal sealed class CardNameXfrEntity : ICardNameXfrEntity
{
    public string CardName { get; init; }
}
