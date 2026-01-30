using System.Security.Claims;
using System.Threading.Tasks;
using App.MtgDiscovery.GraphQL.Authentication;
using Lib.MtgDiscovery.Entry.Entities.Collections;
using Lib.Shared.DataModels.Entities.Args.Collections;

namespace App.MtgDiscovery.GraphQL.Actions.Mappers.Collections;

internal sealed class CreateCollectionArgsMapper : ICreateCollectionArgsMapper
{
    public Task<ICreateCollectionArgsEntity> Map(ClaimsPrincipal claimsPrincipal, ICreateCollectionArgEntity createCollection)
    {
        AuthUserArgEntity authUser = new(claimsPrincipal);

        ICreateCollectionArgsEntity entity = new CreateCollectionArgsEntity
        {
            AuthUser = authUser,
            CreateCollection = createCollection
        };

        return Task.FromResult(entity);
    }
}
