using Lib.Adapter.Collections.Apis.Entities;

namespace Lib.Aggregator.Collections.Queries.Entities;

internal sealed class CollectionIdXfrEntity : ICollectionIdXfrEntity
{
    public string CollectionId { get; init; }
    public string OwnerId { get; init; }
    public string CacheKey => $"collection:{CollectionId}:owner:{OwnerId}";
}
