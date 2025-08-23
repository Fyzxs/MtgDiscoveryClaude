using System.Collections.Generic;
using Lib.Shared.DataModels.Entities;

namespace Lib.MtgDiscovery.Entry;

internal sealed class EntryCardIdsItrEntity : ICardIdsItrEntity
{
    public ICollection<string> CardIds { get; init; }
}
