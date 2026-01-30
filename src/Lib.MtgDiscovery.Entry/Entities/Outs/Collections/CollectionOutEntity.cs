using System.Collections.Generic;

namespace Lib.MtgDiscovery.Entry.Entities.Outs.Collections;

public sealed class CollectionOutEntity
{
    public string CollectionId { get; init; } = string.Empty;
    public string OwnerId { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string Type { get; init; } = string.Empty;
    public string Visibility { get; init; } = string.Empty;
    public bool IsDefault { get; init; }
    public ICollection<AuthorizedUserOutEntity> AuthorizedUsers { get; init; } = [];
    public string CreatedAt { get; init; } = string.Empty;
    public string UpdatedAt { get; init; } = string.Empty;
}
