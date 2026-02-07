using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Adapter.Collections.Apis.Entities;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Collections;
using Lib.Universal.Primitives;

namespace Lib.Adapter.Collections.Commands.Integrators;

internal sealed class GrantAccessIntegrator : IGrantAccessIntegrator
{
    private readonly TimeInstantString _timestamp;

    public GrantAccessIntegrator()
        : this(new Iso8601UtcNowString())
    { }

    private GrantAccessIntegrator(TimeInstantString timestamp) => _timestamp = timestamp;

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
            GrantedAt = _timestamp,
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
            UpdatedAt = _timestamp
        };

        return Task.FromResult(result);
    }
}
