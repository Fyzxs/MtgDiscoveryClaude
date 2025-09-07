using Lib.Shared.UserInfo.Values;

namespace Lib.Shared.DataModels.Entities;

//TODO There are no other classes here... do these belong here?
public sealed class UserRegistrationItrEntity : IUserRegistrationItrEntity
{
    public UserId UserId { get; init; }
}
