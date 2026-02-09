using System.Collections.Generic;
using Lib.Shared.DataModels.Entities.Oufs.Cards;

namespace Lib.Aggregator.Cards.Tests.Fakes;

public sealed class CardItemCollectionOufEntityFake : ICardItemCollectionOufEntity
{
    public ICollection<ICardItemOufEntity> Data { get; init; } = [];
}
