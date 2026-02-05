using System.Collections.Generic;
using Lib.Adapter.Collections.Apis.Entities;

namespace Lib.Aggregator.Collections.Commands.Entities;

internal sealed class CollectionXfrEntity : ICollectionXfrEntity
{
    public string CollectionId { get; init; }
    public string OwnerId { get; init; }
    public string Name { get; init; }
    public string Type { get; init; }
    public string Visibility { get; init; }
    public bool IsDefault { get; init; }
    public IReadOnlyList<IAuthorizedUserXfrEntity> AuthorizedUsers { get; init; } = [];
    public string CreatedAt { get; init; }
    public string UpdatedAt { get; init; }
    public string CacheKey => $"collection:{CollectionId}:owner:{OwnerId}";
}
