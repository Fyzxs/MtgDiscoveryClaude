using System.Collections.Generic;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.Aggregator.Sets.Entities;

internal sealed class SetItemCollectionItrEntity : ISetItemCollectionItrEntity
{
    public ICollection<ISetItemItrEntity> Data { get; init; }
}
