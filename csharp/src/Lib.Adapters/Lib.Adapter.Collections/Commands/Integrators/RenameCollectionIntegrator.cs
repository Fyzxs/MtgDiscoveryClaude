using System.Threading.Tasks;
using Lib.Adapter.Collections.Apis.Entities;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Collections;
using Lib.Universal.Primitives;

namespace Lib.Adapter.Collections.Commands.Integrators;

internal sealed class RenameCollectionIntegrator : IRenameCollectionIntegrator
{
    private readonly TimeInstantString _timestamp;

    public RenameCollectionIntegrator()
        : this(new Iso8601UtcNowString())
    { }

    private RenameCollectionIntegrator(TimeInstantString timestamp) => _timestamp = timestamp;

    public Task<CollectionExtEntity> Integrate(CollectionExtEntity current, IRenameCollectionXfrEntity change)
    {
        CollectionExtEntity result = new()
        {
            CollectionId = current.CollectionId,
            OwnerId = current.OwnerId,
            Name = change.Name,
            Type = current.Type,
            Visibility = current.Visibility,
            IsDefault = current.IsDefault,
            AuthorizedUsers = current.AuthorizedUsers,
            CreatedAt = current.CreatedAt,
            UpdatedAt = _timestamp
        };

        return Task.FromResult(result);
    }
}
