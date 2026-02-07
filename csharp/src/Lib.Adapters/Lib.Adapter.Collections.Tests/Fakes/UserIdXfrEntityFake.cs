using Lib.Adapter.Collections.Apis.Entities;

namespace Lib.Adapter.Collections.Tests.Fakes;

public sealed class UserIdXfrEntityFake : IUserIdXfrEntity
{
    public string UserId { get; init; } = string.Empty;
    public string CacheKey => $"user:{UserId}";
}
