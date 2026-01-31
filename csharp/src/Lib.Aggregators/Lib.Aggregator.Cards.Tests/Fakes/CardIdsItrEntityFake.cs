using System.Collections.Generic;
using Lib.Shared.DataModels.Entities.Itrs.Cards;

namespace Lib.Aggregator.Cards.Tests.Fakes;

public sealed class CardIdsItrEntityFake : ICardIdsItrEntity
{
    public ICollection<string> CardIds { get; init; } = [];
}
