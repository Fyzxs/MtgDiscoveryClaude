using System.Collections.Generic;
using Lib.Shared.DataModels.Entities;

namespace Lib.MtgDiscovery.Entry.Entities;

internal sealed class EntryCardIdsItrEntity : ICardIdsItrEntity
{
    public ICollection<string> CardIds { get; init; }
}
