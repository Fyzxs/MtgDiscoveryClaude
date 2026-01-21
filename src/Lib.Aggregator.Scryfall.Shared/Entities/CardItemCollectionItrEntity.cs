using System.Collections.Generic;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.Aggregator.Scryfall.Shared.Entities;

public sealed class CardItemCollectionOufEntity : ICardItemCollectionOufEntity
{
    public ICollection<ICardItemItrEntity> Data { get; init; }
}
