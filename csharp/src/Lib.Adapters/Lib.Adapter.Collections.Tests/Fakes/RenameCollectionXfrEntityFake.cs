using Lib.Adapter.Collections.Apis.Entities;

namespace Lib.Adapter.Collections.Tests.Fakes;

public sealed class RenameCollectionXfrEntityFake : IRenameCollectionXfrEntity
{
    public string CollectionId { get; init; } = string.Empty;
    public string OwnerId { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string CacheKey => $"rename-collection:{CollectionId}:owner:{OwnerId}";
}
