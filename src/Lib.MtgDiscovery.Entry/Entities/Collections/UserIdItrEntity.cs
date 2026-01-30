using Lib.Shared.DataModels.Entities.Itrs.User;

namespace Lib.MtgDiscovery.Entry.Entities.Collections;

internal sealed class UserIdItrEntity : IUserIdItrEntity
{
    public string UserId { get; init; }
}
