using Lib.Shared.UserInfo.Values;

namespace Lib.Shared.DataModels.Entities;

public sealed class UserInfoItrEntity : IUserInfoItrEntity
{
    public UserId UserId { get; init; }
    public UserSourceId UserSourceId { get; init; }
    public UserNickname UserNickname { get; init; }
}