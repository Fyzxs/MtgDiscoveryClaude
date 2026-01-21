using System.Collections.Generic;
using Lib.Shared.DataModels.Entities.Itrs.UserWishlistCards;

namespace Lib.MtgDiscovery.Entry.Queries.Entities;

internal sealed class UserWishlistCardsByIdsItrEntity : IUserWishlistCardsByIdsItrEntity
{
    public string UserId { get; init; }
    public ICollection<string> CardIds { get; init; }
}
