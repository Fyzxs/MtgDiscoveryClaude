using System.Collections.Generic;
using Lib.Shared.DataModels.Abstractions;

namespace Lib.Adapter.Collections.Apis.Entities;

public interface ICollectionXfrEntity : IXfrEntity
{
    string CollectionId { get; }
    string OwnerId { get; }
    string Name { get; }
    string Type { get; }
    string Visibility { get; }
    bool IsDefault { get; }
    IReadOnlyList<IAuthorizedUserXfrEntity> AuthorizedUsers { get; }
    string CreatedAt { get; }
    string UpdatedAt { get; }
}
