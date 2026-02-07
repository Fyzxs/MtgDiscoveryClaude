using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.Adapter.Collections.Apis.Entities;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Collections;

namespace Lib.Adapter.Collections.Commands.Mappers;

internal sealed class CollectionXfrToExtMapper : ICollectionXfrToExtMapper
{
    public Task<CollectionExtEntity> Map(ICollectionXfrEntity source)
    {
        CollectionExtEntity result = new()
        {
            CollectionId = source.CollectionId,
            OwnerId = source.OwnerId,
            Name = source.Name,
            Type = source.Type,
            Visibility = source.Visibility,
            IsDefault = source.IsDefault,
            AuthorizedUsers = MapAuthorizedUsers(source.AuthorizedUsers),
            CreatedAt = source.CreatedAt,
            UpdatedAt = source.UpdatedAt
        };

        return Task.FromResult(result);
    }

    private static List<AuthorizedUserExtEntity> MapAuthorizedUsers(IEnumerable<IAuthorizedUserXfrEntity> users)
        => users.Select(user => new AuthorizedUserExtEntity
        {
            UserId = user.UserId,
            Role = user.Role,
            GrantedAt = user.GrantedAt,
            GrantedBy = user.GrantedBy
        }).ToList();
}
