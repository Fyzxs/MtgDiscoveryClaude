using System.Collections.Generic;
using Lib.Shared.DataModels.Entities.Itrs.Collections;

namespace Lib.Adapter.Collections.Tests.Fakes;

internal sealed class CollectionItrEntityFake : ICollectionItrEntity
{
    public string CollectionId { get; init; } = string.Empty;
    public string OwnerId { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string Type { get; init; } = string.Empty;
    public string Visibility { get; init; } = string.Empty;
    public bool IsDefault { get; init; }
    public IReadOnlyList<IAuthorizedUserItrEntity> AuthorizedUsers { get; init; } = [];
    public string CreatedAt { get; init; } = string.Empty;
    public string UpdatedAt { get; init; } = string.Empty;
}
