using System.Security.Claims;
using System.Threading.Tasks;
using App.MtgDiscovery.GraphQL.Authentication;
using Lib.MtgDiscovery.Entry.Entities.Collections;
using Lib.Shared.DataModels.Entities.Args.Collections;

namespace App.MtgDiscovery.GraphQL.Actions.Mappers.Collections;

internal sealed class RevokeCollectionAccessArgsMapper : IRevokeCollectionAccessArgsMapper
{
    public Task<IRevokeCollectionAccessArgsEntity> Map(ClaimsPrincipal claimsPrincipal, IRevokeCollectionAccessArgEntity revokeAccess)
    {
        AuthUserArgEntity authUser = new(claimsPrincipal);

        IRevokeCollectionAccessArgsEntity entity = new RevokeCollectionAccessArgsEntity
        {
            AuthUser = authUser,
            RevokeAccess = revokeAccess
        };

        return Task.FromResult(entity);
    }
}
