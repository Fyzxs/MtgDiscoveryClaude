using System.Threading.Tasks;
using Lib.Adapter.Collections.Apis.Entities;
using Lib.Aggregator.Collections.Commands.Entities;
using Lib.Shared.DataModels.Entities.Itrs.Collections;

namespace Lib.Aggregator.Collections.Commands.Mappers;

internal sealed class RenameCollectionItrToXfrMapper : IRenameCollectionItrToXfrMapper
{
    public Task<IRenameCollectionXfrEntity> Map(IRenameCollectionItrEntity source)
    {
        IRenameCollectionXfrEntity result = new RenameCollectionXfrEntity
        {
            CollectionId = source.CollectionId,
            OwnerId = source.OwnerId,
            Name = source.Name
        };

        return Task.FromResult(result);
    }
}
