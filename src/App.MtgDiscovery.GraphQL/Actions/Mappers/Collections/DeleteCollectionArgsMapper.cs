using System.Security.Claims;
using System.Threading.Tasks;
using App.MtgDiscovery.GraphQL.Authentication;
using Lib.MtgDiscovery.Entry.Entities.Collections;
using Lib.Shared.DataModels.Entities.Args.Collections;

namespace App.MtgDiscovery.GraphQL.Actions.Mappers.Collections;

internal sealed class DeleteCollectionArgsMapper : IDeleteCollectionArgsMapper
{
    public Task<IDeleteCollectionArgsEntity> Map(ClaimsPrincipal claimsPrincipal, IDeleteCollectionArgEntity deleteCollection)
    {
        AuthUserArgEntity authUser = new(claimsPrincipal);

        IDeleteCollectionArgsEntity entity = new DeleteCollectionArgsEntity
        {
            AuthUser = authUser,
            DeleteCollection = deleteCollection
        };

        return Task.FromResult(entity);
    }
}
