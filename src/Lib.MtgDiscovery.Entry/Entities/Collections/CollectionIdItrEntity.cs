using Lib.Shared.DataModels.Entities.Itrs.Collections;

namespace Lib.MtgDiscovery.Entry.Entities.Collections;

internal sealed class CollectionIdItrEntity : ICollectionIdItrEntity
{
    public string CollectionId { get; init; }
    public string OwnerId { get; init; }
}
