using Lib.Shared.DataModels.Abstractions;

namespace Lib.Shared.DataModels.Entities.Oufs.User;

public interface IUserInfoOufEntity : IOufEntity
{
    string UserId { get; }
    string UserSourceId { get; }
    string UserNickname { get; }
}
