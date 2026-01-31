using System.Security.Claims;
using System.Threading.Tasks;
using App.MtgDiscovery.GraphQL.Authentication;
using Lib.MtgDiscovery.Entry.Entities.Collections;
using Lib.Shared.DataModels.Entities.Args.Collections;

namespace App.MtgDiscovery.GraphQL.Actions.Mappers.Collections;

internal sealed class RenameCollectionArgsMapper : IRenameCollectionArgsMapper
{
    public Task<IRenameCollectionArgsEntity> Map(ClaimsPrincipal claimsPrincipal, IRenameCollectionArgEntity renameCollection)
    {
        AuthUserArgEntity authUser = new(claimsPrincipal);

        IRenameCollectionArgsEntity entity = new RenameCollectionArgsEntity
        {
            AuthUser = authUser,
            RenameCollection = renameCollection
        };

        return Task.FromResult(entity);
    }
}
