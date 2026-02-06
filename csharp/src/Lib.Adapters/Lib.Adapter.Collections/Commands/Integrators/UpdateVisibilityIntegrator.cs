using System.Threading.Tasks;
using Lib.Adapter.Collections.Apis.Entities;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Collections;
using Lib.Universal.Primitives;

namespace Lib.Adapter.Collections.Commands.Integrators;

internal sealed class UpdateVisibilityIntegrator : IUpdateVisibilityIntegrator
{
    private readonly TimeInstantString _timestamp;

    public UpdateVisibilityIntegrator()
        : this(new Iso8601UtcNowString())
    { }

    private UpdateVisibilityIntegrator(TimeInstantString timestamp) => _timestamp = timestamp;

    public Task<CollectionExtEntity> Integrate(CollectionExtEntity current, IUpdateCollectionVisibilityXfrEntity change)
    {
        CollectionExtEntity result = new()
        {
            CollectionId = current.CollectionId,
            OwnerId = current.OwnerId,
            Name = current.Name,
            Type = current.Type,
            Visibility = change.Visibility,
            IsDefault = current.IsDefault,
            AuthorizedUsers = current.AuthorizedUsers,
            CreatedAt = current.CreatedAt,
            UpdatedAt = _timestamp
        };

        return Task.FromResult(result);
    }
}
