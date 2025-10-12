using System.Collections.Generic;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.MtgDiscovery.Entry.Queries.Entities;

internal sealed class UserCardsByIdsItrEntity : IUserCardsByIdsItrEntity
{
    public string UserId { get; init; }
    public ICollection<string> CardIds { get; init; }
}
