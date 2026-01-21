using Lib.Shared.DataModels.Abstractions;

namespace Lib.Shared.DataModels.Entities.Itrs.User;

public interface IUserInfoItrEntity : IItrEntity
{
    string UserId { get; }
    string UserSourceId { get; }
    string UserNickname { get; }
    string Email { get; }
}
