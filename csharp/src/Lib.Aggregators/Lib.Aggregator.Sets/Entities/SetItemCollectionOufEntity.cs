using System.Collections.Generic;
using Lib.Shared.DataModels.Entities.Oufs.Sets;

namespace Lib.Aggregator.Sets.Entities;

internal sealed class SetItemCollectionOufEntity : ISetItemCollectionOufEntity
{
    public ICollection<ISetItemOufEntity> Data { get; init; }
}
