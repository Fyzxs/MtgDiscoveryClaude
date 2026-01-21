using System.Collections.Generic;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.Aggregator.Cards.Tests.Fakes;

internal sealed class CardIdsItrEntityFake : ICardIdsItrEntity
{
    public ICollection<string> CardIds { get; init; } = [];
}
