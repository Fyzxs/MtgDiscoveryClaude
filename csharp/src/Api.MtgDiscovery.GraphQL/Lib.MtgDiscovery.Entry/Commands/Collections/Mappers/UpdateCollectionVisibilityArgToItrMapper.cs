using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Commands.Collections.Entities;
using Lib.Shared.DataModels.Entities.Args.Collections;
using Lib.Shared.DataModels.Entities.Itrs.Collections;

namespace Lib.MtgDiscovery.Entry.Commands.Collections.Mappers;

internal sealed class UpdateCollectionVisibilityArgToItrMapper : IUpdateCollectionVisibilityArgToItrMapper
{
    public Task<IUpdateCollectionVisibilityItrEntity> Map(IUpdateCollectionVisibilityArgEntity source1, string userId)
    {
        IUpdateCollectionVisibilityItrEntity result = new UpdateCollectionVisibilityItrEntity
        {
            CollectionId = source1.CollectionId,
            OwnerId = userId,
            Visibility = source1.Visibility.ToLowerInvariant()
        };

        return Task.FromResult(result);
    }
}
