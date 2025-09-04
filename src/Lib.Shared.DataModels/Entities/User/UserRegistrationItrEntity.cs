using Lib.Shared.UserInfo.Values;

namespace Lib.Shared.DataModels.Entities;

public sealed class UserRegistrationItrEntity : IUserRegistrationItrEntity
{
    public UserId UserId { get; init; }
}