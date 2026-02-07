using Lib.Adapter.Collections.Apis.Entities;

namespace Lib.Adapter.Collections.Tests.Fakes;

public sealed class DeleteCollectionXfrEntityFake : IDeleteCollectionXfrEntity
{
    public string CollectionId { get; init; } = string.Empty;
    public string OwnerId { get; init; } = string.Empty;
    public string CacheKey => $"delete-collection:{CollectionId}:owner:{OwnerId}";
}
