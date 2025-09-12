using System.Collections.Generic;
using Lib.Shared.DataModels.Entities;

namespace Lib.MtgDiscovery.Entry.Entities;

internal sealed class EntrySetCodesItrEntity : ISetCodesItrEntity
{
    private readonly List<string> _setCodes;

    public EntrySetCodesItrEntity(List<string> setCodes)
    {
        _setCodes = setCodes;
    }

    public IReadOnlyCollection<string> SetCodes => _setCodes;
}
