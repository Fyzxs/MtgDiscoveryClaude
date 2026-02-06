using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Adapter.Collections.Apis.Entities;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Collections;
using Lib.Universal.Primitives;

namespace Lib.Adapter.Collections.Commands.Integrators;

internal sealed class TransferOwnershipIntegrator : ITransferOwnershipIntegrator
{
    private readonly TimeInstantString _timestamp;

    public TransferOwnershipIntegrator()
        : this(new Iso8601UtcNowString())
    { }

    private TransferOwnershipIntegrator(TimeInstantString timestamp) => _timestamp = timestamp;

    public Task<CollectionExtEntity> Integrate(CollectionExtEntity current, ITransferCollectionOwnershipXfrEntity change)
    {
        List<AuthorizedUserExtEntity> authorizedUsers = [.. current.AuthorizedUsers];
        authorizedUsers.RemoveAll(u => u.UserId == change.TargetUserId);

        authorizedUsers.Add(new AuthorizedUserExtEntity
        {
            UserId = change.CurrentOwnerId,
            Role = "admin",
            GrantedAt = _timestamp,
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
            UpdatedAt = _timestamp
        };

        return Task.FromResult(result);
    }
}
