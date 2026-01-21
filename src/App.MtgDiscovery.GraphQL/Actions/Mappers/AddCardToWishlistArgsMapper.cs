using System.Security.Claims;
using System.Threading.Tasks;
using App.MtgDiscovery.GraphQL.Authentication;
using Lib.MtgDiscovery.Entry.Entities;
using Lib.Shared.DataModels.Entities.Args.UserWishlistCards;

namespace App.MtgDiscovery.GraphQL.Actions.Mappers;

internal sealed class AddCardToWishlistArgsMapper : IAddCardToWishlistArgsMapper
{
    public Task<IAddCardToWishlistArgsEntity> Map(ClaimsPrincipal claimsPrincipal, IAddUserWishlistCardArgEntity addUserWishlistCard)
    {
        AuthUserArgEntity authUser = new(claimsPrincipal);

        IAddCardToWishlistArgsEntity entity = new AddCardToWishlistArgsEntity
        {
            AuthUser = authUser,
            AddUserWishlistCard = addUserWishlistCard
        };

        return Task.FromResult(entity);
    }
}
