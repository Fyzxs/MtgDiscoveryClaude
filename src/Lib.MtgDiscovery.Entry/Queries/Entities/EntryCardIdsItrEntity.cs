using System.Collections.Generic;
using Lib.Shared.DataModels.Entities.Itrs.Cards;

namespace Lib.MtgDiscovery.Entry.Queries.Entities;

internal sealed class EntryCardIdsItrEntity : ICardIdsItrEntity
{
    public ICollection<string> CardIds { get; init; }
}
