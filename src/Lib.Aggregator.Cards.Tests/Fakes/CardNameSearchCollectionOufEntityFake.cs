using System.Collections.Generic;
using Lib.Shared.DataModels.Entities.Itrs.Cards;
using Lib.Shared.DataModels.Entities.Oufs.Cards;

namespace Lib.Aggregator.Cards.Tests.Fakes;

internal sealed class CardNameSearchCollectionOufEntityFake : ICardNameSearchCollectionOufEntity
{
    public ICollection<ICardNameSearchResultItrEntity> Names { get; init; } = [];
}
