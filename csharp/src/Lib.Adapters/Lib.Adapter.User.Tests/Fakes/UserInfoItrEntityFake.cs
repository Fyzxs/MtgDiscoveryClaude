using Lib.Shared.DataModels.Entities.Itrs.User;

namespace Lib.Adapter.User.Tests.Fakes;

internal sealed class UserInfoItrEntityFake : IUserInfoItrEntity
{
    public string UserId { get; init; } = string.Empty;
    public string UserSourceId { get; init; } = string.Empty;
    public string UserNickname { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
}
