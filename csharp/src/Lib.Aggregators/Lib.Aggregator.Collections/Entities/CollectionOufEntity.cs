using System.Collections.Generic;
using Lib.Shared.DataModels.Entities.Oufs.Collections;

namespace Lib.Aggregator.Collections.Entities;

internal sealed class CollectionOufEntity : ICollectionOufEntity
{
    public string CollectionId { get; init; }
    public string OwnerId { get; init; }
    public string Name { get; init; }
    public string Type { get; init; }
    public string Visibility { get; init; }
    public bool IsDefault { get; init; }
    public IReadOnlyList<IAuthorizedUserOufEntity> AuthorizedUsers { get; init; }
    public string CreatedAt { get; init; }
    public string UpdatedAt { get; init; }
}
