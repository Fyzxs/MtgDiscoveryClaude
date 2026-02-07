using Lib.Adapter.Collections.Apis.Entities;

namespace Lib.Aggregator.Collections.Commands.Entities;

internal sealed class DeleteCollectionXfrEntity : IDeleteCollectionXfrEntity
{
    public string CollectionId { get; init; }
    public string OwnerId { get; init; }
    public string CacheKey => $"delete_collection:{CollectionId}:owner:{OwnerId}";
}
