using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Commands.Collections.Entities;
using Lib.Shared.DataModels.Entities.Args.Collections;
using Lib.Shared.DataModels.Entities.Itrs.Collections;

namespace Lib.MtgDiscovery.Entry.Commands.Collections.Mappers;

internal sealed class RenameCollectionArgToItrMapper : IRenameCollectionArgToItrMapper
{
    public Task<IRenameCollectionItrEntity> Map(IRenameCollectionArgEntity source1, string userId)
    {
        IRenameCollectionItrEntity result = new RenameCollectionItrEntity
        {
            CollectionId = source1.CollectionId,
            OwnerId = userId,
            Name = source1.Name
        };

        return Task.FromResult(result);
    }
}
