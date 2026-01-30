using System.Collections.Generic;
using Lib.Shared.DataModels.Entities.Itrs.Collections;

namespace Lib.MtgDiscovery.Entry.Commands.Collections.Entities;

internal sealed class CollectionItrEntity : ICollectionItrEntity
{
    public string CollectionId { get; init; }
    public string OwnerId { get; init; }
    public string Name { get; init; }
    public string Type { get; init; }
    public string Visibility { get; init; }
    public bool IsDefault { get; init; }
    public IReadOnlyList<IAuthorizedUserItrEntity> AuthorizedUsers { get; init; } = [];
    public string CreatedAt { get; init; }
    public string UpdatedAt { get; init; }
}
