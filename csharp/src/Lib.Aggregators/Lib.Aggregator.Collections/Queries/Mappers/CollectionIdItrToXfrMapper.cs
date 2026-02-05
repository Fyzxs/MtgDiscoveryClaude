using System.Threading.Tasks;
using Lib.Adapter.Collections.Apis.Entities;
using Lib.Aggregator.Collections.Queries.Entities;
using Lib.Shared.DataModels.Entities.Itrs.Collections;

namespace Lib.Aggregator.Collections.Queries.Mappers;

internal sealed class CollectionIdItrToXfrMapper : ICollectionIdItrToXfrMapper
{
    public Task<ICollectionIdXfrEntity> Map(ICollectionIdItrEntity source)
    {
        ICollectionIdXfrEntity result = new CollectionIdXfrEntity
        {
            CollectionId = source.CollectionId,
            OwnerId = source.OwnerId
        };
        return Task.FromResult(result);
    }
}
