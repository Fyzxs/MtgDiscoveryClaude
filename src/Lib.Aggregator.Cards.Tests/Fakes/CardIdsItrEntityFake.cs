using System.Collections.Generic;

namespace Lib.Aggregator.Cards.Tests.Fakes;

internal sealed class CardIdsItrEntityFake : ICardIdsItrEntity
{
    public ICollection<string> CardIds { get; init; } = [];
}
