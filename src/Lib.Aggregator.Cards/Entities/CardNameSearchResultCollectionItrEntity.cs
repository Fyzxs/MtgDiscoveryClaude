using System.Collections.Generic;
using Lib.Shared.DataModels.Entities;

namespace Lib.Aggregator.Cards.Entities;

internal sealed class CardNameSearchResultCollectionItrEntity : ICardNameSearchResultCollectionItrEntity
{
    public ICollection<ICardNameSearchResultItrEntity> Names { get; init; }
}
