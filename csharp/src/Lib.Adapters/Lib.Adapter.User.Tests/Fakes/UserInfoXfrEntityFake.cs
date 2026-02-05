using Lib.Adapter.User.Apis.Entities;

namespace Lib.Adapter.User.Tests.Fakes;

internal sealed class UserInfoXfrEntityFake : IUserInfoXfrEntity
{
    public string UserId { get; init; } = string.Empty;
    public string UserSourceId { get; init; } = string.Empty;
    public string UserNickname { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string CacheKey => $"user:{UserId}";
}
