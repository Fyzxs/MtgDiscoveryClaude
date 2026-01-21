using System.Collections.Generic;
using Lib.Shared.DataModels.Entities;

namespace Lib.MtgDiscovery.Entry.Entities;

internal sealed class EntrySetIdsItrEntity : ISetIdsItrEntity
{
    private readonly List<string> _setIds;

    public EntrySetIdsItrEntity(List<string> setIds)
    {
        _setIds = setIds;
    }

    public IReadOnlyCollection<string> SetIds => _setIds;
}
