using System.Security.Claims;
using System.Threading.Tasks;
using App.MtgDiscovery.GraphQL.Authentication;
using App.MtgDiscovery.GraphQL.Entities.Args;
using Lib.Shared.DataModels.Entities.Args.Collections;

namespace App.MtgDiscovery.GraphQL.Actions.Mappers.Collections;

internal sealed class CollectionIdArgsMapper : ICollectionIdArgsMapper
{
    public Task<ICollectionIdArgEntity> Map(ClaimsPrincipal claimsPrincipal, string collectionId)
    {
        AuthUserArgEntity authUser = new(claimsPrincipal);

        ICollectionIdArgEntity entity = new CollectionIdArgEntity
        {
            UserId = authUser.UserId,
            CollectionId = collectionId
        };

        return Task.FromResult(entity);
    }
}
