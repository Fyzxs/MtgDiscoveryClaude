using System.Collections.Generic;
using Lib.Shared.DataModels.Entities.Itrs.Cards;
using Lib.Shared.DataModels.Entities.Oufs.Cards;

namespace Lib.Aggregator.Cards.Entities;

internal sealed class CardNameSearchCollectionOufEntity : ICardNameSearchCollectionOufEntity
{
    public ICollection<ICardNameSearchResultItrEntity> Names { get; init; }
}
