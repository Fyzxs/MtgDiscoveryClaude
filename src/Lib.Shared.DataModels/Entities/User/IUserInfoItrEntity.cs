using Lib.Shared.UserInfo.Values;

namespace Lib.Shared.DataModels.Entities;

public interface IUserInfoItrEntity
{
    UserId UserId { get; }
    UserSourceId UserSourceId { get; }
    UserNickname UserNickname { get; }
}