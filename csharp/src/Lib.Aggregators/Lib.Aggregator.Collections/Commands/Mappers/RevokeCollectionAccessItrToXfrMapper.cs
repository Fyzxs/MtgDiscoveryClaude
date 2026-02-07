using System.Threading.Tasks;
using Lib.Adapter.Collections.Apis.Entities;
using Lib.Aggregator.Collections.Commands.Entities;
using Lib.Shared.DataModels.Entities.Itrs.Collections;

namespace Lib.Aggregator.Collections.Commands.Mappers;

internal sealed class RevokeCollectionAccessItrToXfrMapper : IRevokeCollectionAccessItrToXfrMapper
{
    public Task<IRevokeCollectionAccessXfrEntity> Map(IRevokeCollectionAccessItrEntity source)
    {
        IRevokeCollectionAccessXfrEntity result = new RevokeCollectionAccessXfrEntity
        {
            CollectionId = source.CollectionId,
            RevokerUserId = source.RevokerUserId,
            TargetUserId = source.TargetUserId
        };

        return Task.FromResult(result);
    }
}
