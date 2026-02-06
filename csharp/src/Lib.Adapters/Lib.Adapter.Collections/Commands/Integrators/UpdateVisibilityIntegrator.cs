using System;
using System.Threading.Tasks;
using Lib.Adapter.Collections.Apis.Entities;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Collections;

namespace Lib.Adapter.Collections.Commands.Integrators;

internal sealed class UpdateVisibilityIntegrator : IUpdateVisibilityIntegrator
{
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
            UpdatedAt = DateTime.UtcNow.ToString("o")
        };

        return Task.FromResult(result);
    }
}
