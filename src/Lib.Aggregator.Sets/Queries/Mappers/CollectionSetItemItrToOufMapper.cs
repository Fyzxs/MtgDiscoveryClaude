using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Aggregator.Sets.Entities;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.Aggregator.Sets.Queries.Mappers;

internal sealed class CollectionSetItemItrToOufMapper : ICollectionSetItemItrToOufMapper
{
    public Task<ISetItemCollectionOufEntity> Map(IEnumerable<ISetItemItrEntity> sets)
    {
        ICollection<ISetItemItrEntity> setCollection = [.. sets];
        ISetItemCollectionOufEntity result = new SetItemCollectionOufEntity { Data = setCollection };

        return Task.FromResult(result);
    }
}
