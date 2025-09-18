using System.Collections.Generic;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.Aggregator.Cards.Tests.Fakes;

internal sealed class CardItemCollectionItrEntityFake : ICardItemCollectionItrEntity
{
    public ICollection<ICardItemItrEntity> Data { get; init; } = [];
}
