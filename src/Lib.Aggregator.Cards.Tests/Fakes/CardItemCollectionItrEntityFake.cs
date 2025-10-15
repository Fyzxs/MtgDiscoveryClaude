using System.Collections.Generic;

namespace Lib.Aggregator.Cards.Tests.Fakes;

internal sealed class CardItemCollectionItrEntityFake : ICardItemCollectionOufEntity
{
    public ICollection<ICardItemItrEntity> Data { get; init; } = [];
}
