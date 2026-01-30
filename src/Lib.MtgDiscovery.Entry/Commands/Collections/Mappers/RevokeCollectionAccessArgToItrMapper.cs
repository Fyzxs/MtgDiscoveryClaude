using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Commands.Collections.Entities;
using Lib.Shared.DataModels.Entities.Args.Collections;
using Lib.Shared.DataModels.Entities.Itrs.Collections;

namespace Lib.MtgDiscovery.Entry.Commands.Collections.Mappers;

internal sealed class RevokeCollectionAccessArgToItrMapper : IRevokeCollectionAccessArgToItrMapper
{
    public Task<IRevokeCollectionAccessItrEntity> Map(IRevokeCollectionAccessArgEntity source1, string userId)
    {
        IRevokeCollectionAccessItrEntity result = new RevokeCollectionAccessItrEntity
        {
            CollectionId = source1.CollectionId,
            RevokerUserId = userId,
            TargetUserId = source1.TargetUserId
        };

        return Task.FromResult(result);
    }
}
