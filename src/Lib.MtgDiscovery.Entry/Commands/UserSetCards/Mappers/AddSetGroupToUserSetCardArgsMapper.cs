using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Args.UserSetCards;

namespace Lib.MtgDiscovery.Entry.Commands.UserSetCards.Mappers;

internal sealed class AddSetGroupToUserSetCardArgsMapper : IAddSetGroupToUserSetCardArgsMapper
{
    public Task<IAddSetGroupToUserSetCardArgsEntity> Map(IAuthUserArgEntity authUser, IAddSetGroupToUserSetCardArgEntity addSetGroupToUserSetCard)
    {
        IAddSetGroupToUserSetCardArgsEntity entity = new AddSetGroupToUserSetCardArgsEntity
        {
            AuthUser = authUser,
            AddSetGroupToUserSetCard = addSetGroupToUserSetCard
        };

        return Task.FromResult(entity);
    }
}
