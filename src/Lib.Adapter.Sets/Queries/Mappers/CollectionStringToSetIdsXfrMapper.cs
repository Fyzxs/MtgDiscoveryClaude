using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.Adapter.Sets.Apis.Entities;
using Lib.Adapter.Sets.Queries.Entities;

namespace Lib.Adapter.Sets.Queries.Mappers;

internal sealed class CollectionStringToSetIdsXfrMapper : ICollectionStringToSetIdsXfrMapper
{
    public Task<ISetIdsXfrEntity> Map(IEnumerable<string> setIds)
    {
        ISetIdsXfrEntity entity = new SetIdsXfrEntity
        {
            SetIds = setIds.ToList()
        };

        return Task.FromResult(entity);
    }
}
