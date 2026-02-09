using Lib.Adapter.User.Apis.Entities;

namespace Lib.Aggregator.User.Commands.Entities;

/// <summary>
/// Concrete XfrEntity for user info passed to adapter.
/// </summary>
internal sealed class UserInfoXfrEntity : IUserInfoXfrEntity
{
    public string UserId { get; init; }
    public string UserSourceId { get; init; }
    public string UserNickname { get; init; }
    public string Email { get; init; }
    public string CacheKey => $"user:{UserId}";
}
