using System.Threading.Tasks;
using Lib.Adapter.Collections.Apis.Entities;
using Lib.Aggregator.Collections.Commands.Entities;
using Lib.Shared.DataModels.Entities.Itrs.Collections;

namespace Lib.Aggregator.Collections.Commands.Mappers;

internal sealed class UpdateCollectionVisibilityItrToXfrMapper : IUpdateCollectionVisibilityItrToXfrMapper
{
    public Task<IUpdateCollectionVisibilityXfrEntity> Map(IUpdateCollectionVisibilityItrEntity source)
    {
        IUpdateCollectionVisibilityXfrEntity result = new UpdateCollectionVisibilityXfrEntity
        {
            CollectionId = source.CollectionId,
            OwnerId = source.OwnerId,
            Visibility = source.Visibility
        };

        return Task.FromResult(result);
    }
}
