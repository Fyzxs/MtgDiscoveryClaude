using System.Collections.Generic;
using Lib.Shared.DataModels.Entities;

namespace Lib.Aggregator.Cards.Tests.Fakes;

internal sealed class FakeCardIdsItrEntity : ICardIdsItrEntity
{
    public ICollection<string> CardIds { get; init; } = new List<string>();
}