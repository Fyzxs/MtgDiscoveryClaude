using Lib.Adapter.Collections.Apis.Entities;

namespace Lib.Aggregator.Collections.Commands.Entities;

internal sealed class RevokeCollectionAccessXfrEntity : IRevokeCollectionAccessXfrEntity
{
    public string CollectionId { get; init; }
    public string RevokerUserId { get; init; }
    public string TargetUserId { get; init; }
    public string CacheKey => $"revoke_access:{CollectionId}:revoker:{RevokerUserId}:target:{TargetUserId}";
}
