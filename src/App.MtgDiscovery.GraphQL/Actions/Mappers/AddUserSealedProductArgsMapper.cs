using System.Security.Claims;
using System.Threading.Tasks;
using App.MtgDiscovery.GraphQL.Authentication;
using Lib.MtgDiscovery.Entry.Entities;
using Lib.Shared.DataModels.Entities.Args.UserSealedProducts;

namespace App.MtgDiscovery.GraphQL.Actions.Mappers;

internal sealed class AddUserSealedProductArgsMapper : IAddUserSealedProductArgsMapper
{
    public Task<IAddSealedProductToCollectionArgsEntity> Map(ClaimsPrincipal claimsPrincipal, IAddUserSealedProductArgEntity addUserSealedProduct)
    {
        AuthUserArgEntity authUser = new(claimsPrincipal);

        IAddSealedProductToCollectionArgsEntity entity = new AddSealedProductToCollectionArgsEntity
        {
            AuthUser = authUser,
            AddUserSealedProduct = addUserSealedProduct
        };

        return Task.FromResult(entity);
    }
}
