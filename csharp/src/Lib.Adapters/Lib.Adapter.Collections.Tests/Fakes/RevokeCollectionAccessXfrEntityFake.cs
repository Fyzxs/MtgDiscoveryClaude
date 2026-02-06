using Lib.Adapter.Collections.Apis.Entities;

namespace Lib.Adapter.Collections.Tests.Fakes;

public sealed class RevokeCollectionAccessXfrEntityFake : IRevokeCollectionAccessXfrEntity
{
    public string CollectionId { get; init; } = string.Empty;
    public string RevokerUserId { get; init; } = string.Empty;
    public string TargetUserId { get; init; } = string.Empty;
    public string CacheKey => $"revoke-access:{CollectionId}:revoker:{RevokerUserId}:target:{TargetUserId}";
}
