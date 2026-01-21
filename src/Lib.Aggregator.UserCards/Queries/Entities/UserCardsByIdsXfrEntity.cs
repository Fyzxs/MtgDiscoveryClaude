using System.Collections.Generic;
using Lib.Adapter.UserCards.Apis.Entities;

namespace Lib.Aggregator.UserCards.Queries.Entities;

internal sealed class UserCardsByIdsXfrEntity : IUserCardsByIdsXfrEntity
{
    public string UserId { get; init; }
    public ICollection<string> CardIds { get; init; }
}
