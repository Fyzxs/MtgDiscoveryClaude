using System.Collections.Generic;
using Lib.Shared.DataModels.Entities;

namespace Lib.Adapter.Cards.Tests.Fakes;

internal sealed class CardIdsItrEntityFake : ICardIdsItrEntity
{
    public ICollection<string> CardIds { get; init; } = [];
}
