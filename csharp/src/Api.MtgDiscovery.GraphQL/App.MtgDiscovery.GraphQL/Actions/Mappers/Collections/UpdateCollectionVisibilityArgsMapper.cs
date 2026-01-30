using System.Security.Claims;
using System.Threading.Tasks;
using App.MtgDiscovery.GraphQL.Authentication;
using Lib.MtgDiscovery.Entry.Entities.Collections;
using Lib.Shared.DataModels.Entities.Args.Collections;

namespace App.MtgDiscovery.GraphQL.Actions.Mappers.Collections;

internal sealed class UpdateCollectionVisibilityArgsMapper : IUpdateCollectionVisibilityArgsMapper
{
    public Task<IUpdateCollectionVisibilityArgsEntity> Map(ClaimsPrincipal claimsPrincipal, IUpdateCollectionVisibilityArgEntity updateVisibility)
    {
        AuthUserArgEntity authUser = new(claimsPrincipal);

        IUpdateCollectionVisibilityArgsEntity entity = new UpdateCollectionVisibilityArgsEntity
        {
            AuthUser = authUser,
            UpdateVisibility = updateVisibility
        };

        return Task.FromResult(entity);
    }
}
