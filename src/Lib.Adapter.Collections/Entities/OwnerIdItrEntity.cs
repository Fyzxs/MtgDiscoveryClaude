using Lib.Shared.DataModels.Entities.Itrs.Collections;

namespace Lib.Adapter.Collections.Entities;

internal sealed class OwnerIdItrEntity : IOwnerIdItrEntity
{
    public string OwnerId { get; init; }
}
