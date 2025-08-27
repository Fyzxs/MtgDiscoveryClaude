using System.Collections.Generic;
using Lib.Shared.DataModels.Entities;

namespace Lib.Aggregator.Sets.Entities;

internal sealed class SetItemCollectionItrEntity : ISetItemCollectionItrEntity
{
    public ICollection<ISetItemItrEntity> Data { get; init; }

    IReadOnlyCollection<ISetItemItrEntity> ISetItemCollectionItrEntity.Data => (IReadOnlyCollection<ISetItemItrEntity>)Data;
}
