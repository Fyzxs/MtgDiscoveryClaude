using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Adapter.Collections.Apis.Entities;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Collections;

namespace Lib.Adapter.Collections.Commands.Integrators;

internal sealed class RevokeAccessIntegrator : IRevokeAccessIntegrator
{
    public Task<CollectionExtEntity> Integrate(CollectionExtEntity current, IRevokeCollectionAccessXfrEntity change)
    {
        List<AuthorizedUserExtEntity> authorizedUsers = [.. current.AuthorizedUsers];
        authorizedUsers.RemoveAll(u => u.UserId == change.TargetUserId);

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
