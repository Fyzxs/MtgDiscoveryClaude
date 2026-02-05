using Lib.Adapter.Collections.Apis.Entities;

namespace Lib.Aggregator.Collections.Queries.Entities;

internal sealed class UserIdXfrEntity : IUserIdXfrEntity
{
    public string UserId { get; init; }
    public string CacheKey => $"user:{UserId}";
}
