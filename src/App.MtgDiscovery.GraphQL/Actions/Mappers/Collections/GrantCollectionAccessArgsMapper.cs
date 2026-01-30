using System.Security.Claims;
using System.Threading.Tasks;
using App.MtgDiscovery.GraphQL.Authentication;
using Lib.MtgDiscovery.Entry.Entities.Collections;
using Lib.Shared.DataModels.Entities.Args.Collections;

namespace App.MtgDiscovery.GraphQL.Actions.Mappers.Collections;

internal sealed class GrantCollectionAccessArgsMapper : IGrantCollectionAccessArgsMapper
{
    public Task<IGrantCollectionAccessArgsEntity> Map(ClaimsPrincipal claimsPrincipal, IGrantCollectionAccessArgEntity grantAccess)
    {
        AuthUserArgEntity authUser = new(claimsPrincipal);

        IGrantCollectionAccessArgsEntity entity = new GrantCollectionAccessArgsEntity
        {
            AuthUser = authUser,
            GrantAccess = grantAccess
        };

        return Task.FromResult(entity);
    }
}
