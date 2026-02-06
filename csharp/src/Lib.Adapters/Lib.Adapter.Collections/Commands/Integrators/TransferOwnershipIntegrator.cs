using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Adapter.Collections.Apis.Entities;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Collections;

namespace Lib.Adapter.Collections.Commands.Integrators;

internal sealed class TransferOwnershipIntegrator : ITransferOwnershipIntegrator
{
    public Task<CollectionExtEntity> Integrate(CollectionExtEntity current, ITransferCollectionOwnershipXfrEntity change)
    {
        List<AuthorizedUserExtEntity> authorizedUsers = [.. current.AuthorizedUsers];
        authorizedUsers.RemoveAll(u => u.UserId == change.TargetUserId);

        string nowTimestamp = DateTime.UtcNow.ToString("o");
        authorizedUsers.Add(new AuthorizedUserExtEntity
        {
            UserId = change.CurrentOwnerId,
            Role = "admin",
            GrantedAt = nowTimestamp,
            GrantedBy = change.CurrentOwnerId
        });

        CollectionExtEntity result = new()
        {
            CollectionId = current.CollectionId,
            OwnerId = change.TargetUserId,
            Name = current.Name,
            Type = current.Type,
            Visibility = current.Visibility,
            IsDefault = false,
            AuthorizedUsers = authorizedUsers,
            CreatedAt = current.CreatedAt,
            UpdatedAt = nowTimestamp
        };

        return Task.FromResult(result);
    }
}
