using System.Security.Claims;
using System.Threading.Tasks;
using App.MtgDiscovery.GraphQL.Authentication;
using Lib.MtgDiscovery.Entry.Entities.Collections;

namespace App.MtgDiscovery.GraphQL.Actions.Mappers.Collections;

internal sealed class GetCollectionAccessListArgsMapper : IGetCollectionAccessListArgsMapper
{
    public Task<IGetCollectionAccessListArgsEntity> Map(ClaimsPrincipal claimsPrincipal, string collectionId)
    {
        AuthUserArgEntity authUser = new(claimsPrincipal);

        IGetCollectionAccessListArgsEntity entity = new GetCollectionAccessListArgsEntity
        {
            AuthUser = authUser,
            CollectionId = collectionId
        };

        return Task.FromResult(entity);
    }
}
