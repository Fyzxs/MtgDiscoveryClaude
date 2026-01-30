using System.Collections.Generic;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Entities;

namespace Lib.Adapter.Collections.Entities;

internal sealed class CollectionXfrEntity
{
    public string CollectionId { get; init; }
    public string OwnerId { get; init; }
    public string Name { get; init; }
    public string Type { get; init; }
    public string Visibility { get; init; }
    public bool IsDefault { get; init; }
    public IEnumerable<AuthorizedUserExtEntity> AuthorizedUsers { get; init; } = [];
    public string CreatedAt { get; init; }
    public string UpdatedAt { get; init; }
}
