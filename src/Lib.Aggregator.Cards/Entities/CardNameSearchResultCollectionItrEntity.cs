using System.Collections.Generic;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.Aggregator.Cards.Entities;

internal sealed class CardNameSearchResultCollectionOufEntity : ICardNameSearchResultCollectionOufEntity
{
    public ICollection<ICardNameSearchResultItrEntity> Names { get; init; }
}
