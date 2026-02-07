using System.Threading.Tasks;
using Lib.Adapter.Collections.Apis.Entities;
using Lib.Aggregator.Collections.Commands.Entities;
using Lib.Shared.DataModels.Entities.Itrs.Collections;

namespace Lib.Aggregator.Collections.Commands.Mappers;

internal sealed class DeleteCollectionItrToXfrMapper : IDeleteCollectionItrToXfrMapper
{
    public Task<IDeleteCollectionXfrEntity> Map(IDeleteCollectionItrEntity source)
    {
        IDeleteCollectionXfrEntity result = new DeleteCollectionXfrEntity
        {
            CollectionId = source.CollectionId,
            OwnerId = source.OwnerId
        };

        return Task.FromResult(result);
    }
}
