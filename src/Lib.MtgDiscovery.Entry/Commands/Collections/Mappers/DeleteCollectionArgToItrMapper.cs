using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Commands.Collections.Entities;
using Lib.Shared.DataModels.Entities.Args.Collections;
using Lib.Shared.DataModels.Entities.Itrs.Collections;

namespace Lib.MtgDiscovery.Entry.Commands.Collections.Mappers;

internal sealed class DeleteCollectionArgToItrMapper : IDeleteCollectionArgToItrMapper
{
    public Task<IDeleteCollectionItrEntity> Map(IDeleteCollectionArgEntity source1, string userId)
    {
        IDeleteCollectionItrEntity result = new DeleteCollectionItrEntity
        {
            CollectionId = source1.CollectionId,
            OwnerId = userId
        };

        return Task.FromResult(result);
    }
}
