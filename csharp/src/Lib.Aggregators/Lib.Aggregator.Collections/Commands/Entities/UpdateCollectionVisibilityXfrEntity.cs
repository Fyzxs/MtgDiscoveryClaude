using Lib.Adapter.Collections.Apis.Entities;

namespace Lib.Aggregator.Collections.Commands.Entities;

internal sealed class UpdateCollectionVisibilityXfrEntity : IUpdateCollectionVisibilityXfrEntity
{
    public string CollectionId { get; init; }
    public string OwnerId { get; init; }
    public string Visibility { get; init; }
    public string CacheKey => $"update_visibility:{CollectionId}:owner:{OwnerId}";
}
