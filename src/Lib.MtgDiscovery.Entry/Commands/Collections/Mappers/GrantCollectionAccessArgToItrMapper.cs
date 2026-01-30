using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Commands.Collections.Entities;
using Lib.Shared.DataModels.Entities.Args.Collections;
using Lib.Shared.DataModels.Entities.Itrs.Collections;

namespace Lib.MtgDiscovery.Entry.Commands.Collections.Mappers;

internal sealed class GrantCollectionAccessArgToItrMapper : IGrantCollectionAccessArgToItrMapper
{
    public Task<IGrantCollectionAccessItrEntity> Map(IGrantCollectionAccessArgEntity source1, string userId)
    {
        IGrantCollectionAccessItrEntity result = new GrantCollectionAccessItrEntity
        {
            CollectionId = source1.CollectionId,
            GrantorUserId = userId,
            TargetUserId = source1.TargetUserId,
            Role = source1.Role.ToLowerInvariant()
        };

        return Task.FromResult(result);
    }
}
