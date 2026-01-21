using System.Security.Claims;
using System.Threading.Tasks;
using App.MtgDiscovery.GraphQL.Authentication;
using App.MtgDiscovery.GraphQL.Entities.Args.UserSealedProducts;
using Lib.MtgDiscovery.Entry.Entities;
using Lib.Shared.DataModels.Entities.Args.UserSealedProducts;

namespace App.MtgDiscovery.GraphQL.Actions.Mappers;

internal sealed class AddUserSealedProductArgsMapper : IAddUserSealedProductArgsMapper
{
    public Task<IAddUserSealedProductArgsEntity> Map(ClaimsPrincipal claimsPrincipal, IAddUserSealedProductClientArgEntity addUserSealedProduct)
    {
        AuthUserArgEntity authUser = new(claimsPrincipal);

        IAddUserSealedProductArgEntity serverEntity = new AddUserSealedProductServerArgEntity
        {
            UserId = authUser.UserId,
            ProductUuid = addUserSealedProduct.ProductUuid,
            SetId = addUserSealedProduct.SetId,
            CountDelta = addUserSealedProduct.CountDelta
        };

        IAddUserSealedProductArgsEntity entity = new AddUserSealedProductArgsEntity
        {
            AuthUser = authUser,
            AddUserSealedProduct = serverEntity
        };

        return Task.FromResult(entity);
    }
}
