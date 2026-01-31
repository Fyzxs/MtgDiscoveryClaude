using System.Collections.Generic;
using Lib.Shared.DataModels.Entities.Itrs.Cards;
using Lib.Shared.DataModels.Entities.Oufs.Cards;

namespace Lib.Aggregator.Cards.Tests.Fakes;

public sealed class CardItemCollectionItrEntityFake : ICardItemCollectionOufEntity
{
    public ICollection<ICardItemItrEntity> Data { get; init; } = [];
}
