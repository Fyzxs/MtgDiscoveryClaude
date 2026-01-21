using System.Collections.Generic;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.MtgDiscovery.Entry.Queries.Entities;

internal sealed class EntrySetIdsItrEntity : ISetIdsItrEntity
{
    public EntrySetIdsItrEntity(ICollection<string> setIds) => SetIds = setIds;

    public ICollection<string> SetIds { get; }
}
