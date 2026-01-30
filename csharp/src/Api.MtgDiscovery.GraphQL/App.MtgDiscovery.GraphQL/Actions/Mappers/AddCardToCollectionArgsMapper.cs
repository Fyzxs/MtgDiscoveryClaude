using System.Security.Claims;
using System.Threading.Tasks;
using App.MtgDiscovery.GraphQL.Authentication;
using Lib.MtgDiscovery.Entry.Entities;
using Lib.Shared.DataModels.Entities.Args.UserCards;

namespace App.MtgDiscovery.GraphQL.Actions.Mappers;

internal sealed class AddCardToCollectionArgsMapper : IAddCardToCollectionArgsMapper
{
    public Task<IAddCardToCollectionArgsEntity> Map(ClaimsPrincipal claimsPrincipal, IAddUserCardArgEntity addUserCard)
    {
        AuthUserArgEntity authUser = new(claimsPrincipal);

        IAddCardToCollectionArgsEntity entity = new AddCardToCollectionArgsEntity
        {
            AuthUser = authUser,
            AddUserCard = addUserCard
        };

        return Task.FromResult(entity);
    }
}
