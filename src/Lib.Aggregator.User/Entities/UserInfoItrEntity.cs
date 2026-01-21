using Lib.Shared.DataModels.Entities;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.Aggregator.User.Entities;

internal sealed class UserInfoItrEntity : IUserInfoItrEntity
{
    public string UserId { get; init; }
    public string UserSourceId { get; init; }
    public string UserNickname { get; init; }
}
