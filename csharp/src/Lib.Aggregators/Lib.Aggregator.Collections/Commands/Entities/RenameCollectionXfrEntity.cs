using Lib.Adapter.Collections.Apis.Entities;

namespace Lib.Aggregator.Collections.Commands.Entities;

internal sealed class RenameCollectionXfrEntity : IRenameCollectionXfrEntity
{
    public string CollectionId { get; init; }
    public string OwnerId { get; init; }
    public string Name { get; init; }
    public string CacheKey => $"rename_collection:{CollectionId}:owner:{OwnerId}";
}
