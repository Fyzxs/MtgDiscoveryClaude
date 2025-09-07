using Lib.Shared.UserInfo.Values;

namespace Lib.Shared.DataModels.Entities;

//TODO There are no other classes here... do these belong here?
public sealed class UserInfoItrEntity : IUserInfoItrEntity
{
    public UserId UserId { get; init; }
    public UserSourceId UserSourceId { get; init; }
    public UserNickname UserNickname { get; init; }
}
