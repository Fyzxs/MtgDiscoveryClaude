using System.Security.Claims;
using System.Threading.Tasks;
using App.MtgDiscovery.GraphQL.Authentication;
using Lib.MtgDiscovery.Entry.Entities;
using Lib.Shared.DataModels.Entities.Args.UserSetCards;

namespace App.MtgDiscovery.GraphQL.Actions.Mappers;

internal sealed class AddSetGroupToUserSetCardArgsMapper : IAddSetGroupToUserSetCardArgsMapper
{
    public Task<IAddSetGroupToUserSetCardArgsEntity> Map(ClaimsPrincipal claimsPrincipal, IAddSetGroupToUserSetCardArgEntity addSetGroupToUserSetCard)
    {
        AuthUserArgEntity authUser = new(claimsPrincipal);

        IAddSetGroupToUserSetCardArgsEntity entity = new AddSetGroupToUserSetCardArgsEntity
        {
            AuthUser = authUser,
            AddSetGroupToUserSetCard = addSetGroupToUserSetCard
        };

        return Task.FromResult(entity);
    }
}
