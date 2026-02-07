using Lib.Adapter.Collections.Apis.Entities;

namespace Lib.Aggregator.Collections.Commands.Entities;

internal sealed class GrantCollectionAccessXfrEntity : IGrantCollectionAccessXfrEntity
{
    public string CollectionId { get; init; }
    public string GrantorUserId { get; init; }
    public string TargetUserId { get; init; }
    public string Role { get; init; }
    public string CacheKey => $"grant_access:{CollectionId}:grantor:{GrantorUserId}:target:{TargetUserId}";
}
