using System.Collections.Generic;
using Lib.Shared.DataModels.Entities.Itrs.Cards;
using Lib.Shared.DataModels.Entities.Oufs.Cards;

namespace Lib.Aggregator.Scryfall.Shared.Entities;

public sealed class CardItemCollectionOufEntity : ICardItemCollectionOufEntity
{
    public ICollection<ICardItemItrEntity> Data { get; init; }
}
