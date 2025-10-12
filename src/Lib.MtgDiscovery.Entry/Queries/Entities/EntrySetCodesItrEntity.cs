using System.Collections.Generic;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.MtgDiscovery.Entry.Queries.Entities;

internal sealed class EntrySetCodesItrEntity : ISetCodesItrEntity
{
    public EntrySetCodesItrEntity(ICollection<string> setCodes) => SetCodes = setCodes;

    public ICollection<string> SetCodes { get; }
}
