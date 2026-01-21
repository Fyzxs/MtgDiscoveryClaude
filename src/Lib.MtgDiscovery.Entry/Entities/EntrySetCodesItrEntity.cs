using System.Collections.Generic;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.MtgDiscovery.Entry.Entities;

internal sealed class EntrySetCodesItrEntity : ISetCodesItrEntity
{
    private readonly ICollection<string> _setCodes;

    public EntrySetCodesItrEntity(ICollection<string> setCodes)
    {
        _setCodes = setCodes;
    }

    public ICollection<string> SetCodes => _setCodes;
}
