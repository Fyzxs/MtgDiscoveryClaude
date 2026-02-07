using Lib.Adapter.Collections.Apis.Entities;

namespace Lib.Adapter.Collections.Tests.Fakes;

public sealed class UpdateCollectionVisibilityXfrEntityFake : IUpdateCollectionVisibilityXfrEntity
{
    public string CollectionId { get; init; } = string.Empty;
    public string OwnerId { get; init; } = string.Empty;
    public string Visibility { get; init; } = string.Empty;
    public string CacheKey => $"update-visibility:{CollectionId}:owner:{OwnerId}";
}
