using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Adapter.Collections.Apis.Entities;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Collections;

namespace Lib.Adapter.Collections.Commands.Integrators;

internal sealed class GrantAccessIntegrator : IGrantAccessIntegrator
{
    public Task<CollectionExtEntity> Integrate(CollectionExtEntity current, IGrantCollectionAccessXfrEntity change)
    {
        List<AuthorizedUserExtEntity> authorizedUsers = [.. current.AuthorizedUsers];
        AuthorizedUserExtEntity existingUser = authorizedUsers.Find(u => u.UserId == change.TargetUserId);

        if (existingUser is not null)
        {
            authorizedUsers.Remove(existingUser);
        }

        authorizedUsers.Add(new AuthorizedUserExtEntity
        {
            UserId = change.TargetUserId,
            Role = change.Role,
            GrantedAt = DateTime.UtcNow.ToString("o"),
            GrantedBy = change.GrantorUserId
        });

        CollectionExtEntity result = new()
        {
            CollectionId = current.CollectionId,
            OwnerId = current.OwnerId,
            Name = current.Name,
            Type = current.Type,
            Visibility = current.Visibility,
            IsDefault = current.IsDefault,
            AuthorizedUsers = authorizedUsers,
            CreatedAt = current.CreatedAt,
            UpdatedAt = DateTime.UtcNow.ToString("o")
        };

        return Task.FromResult(result);
    }
}
