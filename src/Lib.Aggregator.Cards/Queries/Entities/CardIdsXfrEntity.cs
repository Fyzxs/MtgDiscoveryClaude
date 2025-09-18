using System.Collections.Generic;
using Lib.Adapter.Cards.Apis.Entities;

namespace Lib.Aggregator.Cards.Queries.Entities;

internal sealed class CardIdsXfrEntity : ICardIdsXfrEntity
{
    public IEnumerable<string> CardIds { get; init; }
}
