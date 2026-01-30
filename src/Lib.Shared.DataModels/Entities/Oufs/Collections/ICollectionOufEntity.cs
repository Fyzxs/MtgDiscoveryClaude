using System.Collections.Generic;

namespace Lib.Shared.DataModels.Entities.Oufs.Collections;

public interface ICollectionOufEntity
{
    string CollectionId { get; }
    string OwnerId { get; }
    string Name { get; }
    string Type { get; }
    string Visibility { get; }
    bool IsDefault { get; }
    IReadOnlyList<IAuthorizedUserOufEntity> AuthorizedUsers { get; }
    string CreatedAt { get; }
    string UpdatedAt { get; }
}
