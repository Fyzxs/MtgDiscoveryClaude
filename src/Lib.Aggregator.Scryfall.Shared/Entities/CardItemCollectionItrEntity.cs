using System.Collections.Generic;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.Aggregator.Scryfall.Shared.Entities;

public sealed class CardItemCollectionItrEntity : ICardItemCollectionItrEntity
{
    public ICollection<ICardItemItrEntity> Data { get; init; }
}
