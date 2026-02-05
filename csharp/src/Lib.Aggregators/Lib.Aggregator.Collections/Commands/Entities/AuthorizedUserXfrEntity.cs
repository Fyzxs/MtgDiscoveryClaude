using Lib.Adapter.Collections.Apis.Entities;

namespace Lib.Aggregator.Collections.Commands.Entities;

internal sealed class AuthorizedUserXfrEntity : IAuthorizedUserXfrEntity
{
    public string UserId { get; init; }
    public string Role { get; init; }
    public string GrantedAt { get; init; }
    public string GrantedBy { get; init; }
    public string CacheKey => $"authorized_user:{UserId}";
}
