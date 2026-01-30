using Lib.Shared.DataModels.Entities.Itrs.Collections;

namespace Lib.MtgDiscovery.Entry.Entities.Collections;

internal sealed class OwnerIdItrEntity : IOwnerIdItrEntity
{
    public string OwnerId { get; init; }
}
