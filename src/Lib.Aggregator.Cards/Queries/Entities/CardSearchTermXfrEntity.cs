using Lib.Adapter.Cards.Apis.Entities;

namespace Lib.Aggregator.Cards.Queries.Entities;

internal sealed class CardSearchTermXfrEntity : ICardSearchTermXfrEntity
{
    public string SearchTerm { get; init; }
}
