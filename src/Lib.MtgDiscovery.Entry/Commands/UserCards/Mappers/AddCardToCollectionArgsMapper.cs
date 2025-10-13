using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities;
using Lib.Shared.DataModels.Entities.Args;

namespace Lib.MtgDiscovery.Entry.Commands.UserCards.Mappers;

internal sealed class AddCardToCollectionArgsMapper : IAddCardToCollectionArgsMapper
{
    public Task<IAddCardToCollectionArgsEntity> Map(IAuthUserArgEntity authUser, IAddUserCardArgEntity addUserCard)
    {
        IAddCardToCollectionArgsEntity entity = new AddCardToCollectionArgsEntity
        {
            AuthUser = authUser,
            AddUserCard = addUserCard
        };

        return Task.FromResult(entity);
    }
}
