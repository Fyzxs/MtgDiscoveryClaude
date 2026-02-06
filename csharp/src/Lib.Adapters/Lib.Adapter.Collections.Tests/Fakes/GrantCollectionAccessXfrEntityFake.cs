using Lib.Adapter.Collections.Apis.Entities;

namespace Lib.Adapter.Collections.Tests.Fakes;

public sealed class GrantCollectionAccessXfrEntityFake : IGrantCollectionAccessXfrEntity
{
    public string CollectionId { get; init; } = string.Empty;
    public string GrantorUserId { get; init; } = string.Empty;
    public string TargetUserId { get; init; } = string.Empty;
    public string Role { get; init; } = string.Empty;
    public string CacheKey => $"grant-access:{CollectionId}:grantor:{GrantorUserId}:target:{TargetUserId}";
}
