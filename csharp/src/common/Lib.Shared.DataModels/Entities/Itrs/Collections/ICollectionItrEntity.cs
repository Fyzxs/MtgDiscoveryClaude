using System.Collections.Generic;

namespace Lib.Shared.DataModels.Entities.Itrs.Collections;

public interface ICollectionItrEntity
{
    string CollectionId { get; }
    string OwnerId { get; }
    string Name { get; }
    string Type { get; }
    string Visibility { get; }
    bool IsDefault { get; }
    IReadOnlyList<IAuthorizedUserItrEntity> AuthorizedUsers { get; }
    string CreatedAt { get; }
    string UpdatedAt { get; }
}
