using System.Threading.Tasks;
using Lib.Adapter.Collections.Apis.Entities;
using Lib.Aggregator.Collections.Commands.Entities;
using Lib.Shared.DataModels.Entities.Itrs.Collections;

namespace Lib.Aggregator.Collections.Commands.Mappers;

internal sealed class GrantCollectionAccessItrToXfrMapper : IGrantCollectionAccessItrToXfrMapper
{
    public Task<IGrantCollectionAccessXfrEntity> Map(IGrantCollectionAccessItrEntity source)
    {
        IGrantCollectionAccessXfrEntity result = new GrantCollectionAccessXfrEntity
        {
            CollectionId = source.CollectionId,
            GrantorUserId = source.GrantorUserId,
            TargetUserId = source.TargetUserId,
            Role = source.Role
        };

        return Task.FromResult(result);
    }
}
