using System.Collections.Generic;
using Lib.Shared.DataModels.Entities;

namespace Lib.Aggregator.Cards.Entities;

internal sealed class CardItemCollectionItrEntity : ICardItemCollectionItrEntity
{
    public ICollection<ICardItemItrEntity> Data { get; init; }
}