using Lib.Shared.DataModels.Entities;

namespace Lib.MtgDiscovery.Entry.Entities;

internal sealed class UserInfoItrEntity : IUserInfoItrEntity
{
    public string UserId { get; init; }
    public string UserSourceId { get; init; }
    public string UserNickname { get; init; }
}
