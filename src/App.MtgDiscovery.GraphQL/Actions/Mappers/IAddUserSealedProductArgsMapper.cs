using System.Security.Claims;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities;
using Lib.Shared.DataModels.Entities.Args.UserSealedProducts;

namespace App.MtgDiscovery.GraphQL.Actions.Mappers;

internal interface IAddUserSealedProductArgsMapper
{
    Task<IAddSealedProductToCollectionArgsEntity> Map(ClaimsPrincipal claimsPrincipal, IAddUserSealedProductArgEntity addUserSealedProduct);
}
