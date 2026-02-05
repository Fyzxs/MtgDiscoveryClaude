using System.Collections.Generic;
using Lib.Adapter.Collections.Apis.Entities;

namespace Lib.Adapter.Collections.Tests.Fakes;

public sealed class CollectionXfrEntityFake : ICollectionXfrEntity
{
    public string CollectionId { get; init; } = string.Empty;
    public string OwnerId { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string Type { get; init; } = string.Empty;
    public string Visibility { get; init; } = string.Empty;
    public bool IsDefault { get; init; }
    public IReadOnlyList<IAuthorizedUserXfrEntity> AuthorizedUsers { get; init; } = [];
    public string CreatedAt { get; init; } = string.Empty;
    public string UpdatedAt { get; init; } = string.Empty;
    public string CacheKey => $"collection:{CollectionId}:owner:{OwnerId}";
}
