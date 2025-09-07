using System.Collections.Generic;
using Lib.Shared.DataModels.Entities;

namespace Lib.Aggregator.Scryfall.Shared.Entities;

public sealed class CardItemCollectionItrEntity : ICardItemCollectionItrEntity
{
    public ICollection<ICardItemItrEntity> Data { get; init; }
}
