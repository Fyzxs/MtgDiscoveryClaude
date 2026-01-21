using System.Collections.Generic;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.Aggregator.Sets.Entities;

internal sealed class SetItemCollectionOufEntity : ISetItemCollectionOufEntity
{
    public ICollection<ISetItemItrEntity> Data { get; init; }
}
