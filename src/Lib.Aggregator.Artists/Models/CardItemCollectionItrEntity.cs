using System.Collections.Generic;
using Lib.Shared.DataModels.Entities;

namespace Lib.Aggregator.Artists.Models;

internal sealed class CardItemCollectionItrEntity : ICardItemCollectionItrEntity
{
    public ICollection<ICardItemItrEntity> Data { get; init; }
}